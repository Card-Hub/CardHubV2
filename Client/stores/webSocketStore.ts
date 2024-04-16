import { defineStore } from "pinia";
import {
    // HttpTransportType,
    type HubConnection,
    HubConnectionBuilder,
    HubConnectionState,
    LogLevel
} from "@microsoft/signalr";
import { ref } from "vue";
import type { UnoColor } from "~/types";

export const useWebSocketStore = defineStore("webSocket", () => {
    const { $api } = useNuxtApp();

    enum UserType {
        Gameboard,
        Player
    }

    const connection = ref<HubConnection | null>(null);
    const isConnected = computed(() => connection.value !== null && connection.value.state === HubConnectionState.Connected);
    const isPlayer = ref<boolean | null>(null);

    const cards = ref<UNOCard[]>([]);
    const messages = ref<UserMessage[]>([]);
    const users = ref<string[]>([]);
    const canSelectWildColor = ref<boolean>(false);

    const user = ref("");
    const room = ref("");

    const timer = ref<number>(0);

    const runtimeConfig = useRuntimeConfig();
    const reconnectTimeout = runtimeConfig.public.reconnectTimeout;

    // Users have 60 seconds to reconnect to the server
    // using client-side cookies
    const cookieUser = useCookie<string>("user", { maxAge: reconnectTimeout });
    const cookieRoom = useCookie<string>("room", { maxAge: reconnectTimeout });


    // Attempt to create a new room for the Gameboard device
    const tryCreateRoom = async (): Promise<boolean> => {
        try {
            room.value = await $api<string>("game/createroom", { method: "POST" });
            console.log("Room created: ", room.value);
            await joinRoom(user.value, room.value, UserType.Gameboard);
            isPlayer.value = false;
            return true;
        } catch (e) {
            console.log("Failed to create room", e);
        }
        return false;
    };

    // Attempt to join an existing room as a Player device
    const tryJoinRoom = async (): Promise<boolean> => {
        if (user.value && room.value) {
            try {
                const roomCodeExists = await $api<boolean>(`game/verifycode/${ room.value }`, { method: "GET" });
                if (!roomCodeExists) {
                    console.log("Invalid room code");
                    return false;
                }
                await joinRoom(user.value, room.value, UserType.Player);
                isPlayer.value = true;
                return true;
            } catch (e) {
                console.log(e);
            }
        }

        console.log("Failed to join room");
        return false;
    };

    const joinRoom = async (user: string, room: string, userType: UserType): Promise<void> => {
        const webSocketUrl = `${ runtimeConfig.public.baseURL }/${ runtimeConfig.public.hubPath }`;
        try {
            // HubConnection configuration
            // https://learn.microsoft.com/en-us/aspnet/core/signalr/configuration?view=aspnetcore-8.0&tabs=dotnet#configure-client-options
            const joinConnection = new HubConnectionBuilder()
                .withUrl(webSocketUrl)
                .withStatefulReconnect()
                .withAutomaticReconnect()
                .configureLogging(LogLevel.Debug)
                .build();

            joinConnection.on("ReceiveMessage", (userMessage: UserMessage) => {
                messages.value.push(userMessage);
                console.log(userMessage);
            });

            joinConnection.on("ReceiveCard", (fromUser: string, card: UNOCard) => {
                cards.value.push(card as UNOCard);
                console.log(card);
            });

            joinConnection.on("ReceiveCards", (gameCards: UNOCard[]) => {
                cards.value.push(...gameCards);
            });

            joinConnection.on("ReceiveColor", (color: UnoColor) => {
                cards.value[cards.value.length - 1].color = color;
            }); // For gameboard

            joinConnection.on("RequestColor", () => {
                canSelectWildColor.value = true;
            });

            joinConnection.on("SendColorResult", (result: boolean) => {
                canSelectWildColor.value = false;
            });

            joinConnection.on("UsersInRoom", (groupUsers: string[]) => {
                users.value = groupUsers;
            });

            joinConnection.on("StartedGame", (gameCards: UNOCard[]) => {
                cards.value = gameCards;
                navigateTo("/playerview");
            });

            joinConnection.on("PopCard", (card: UNOCard) => {
                console.log("Popped card:", card);
                cards.value = cards.value.filter(c => c.id !== card.id);
            });

            const timeout = ref<NodeJS.Timeout | null>(null);
            joinConnection.on("SetTimer", async (time: number) => {
                if (timeout.value)
                    clearInterval(timeout.value);
                console.log("Timer started:", timer);
                timer.value = time;
                if (time > 0) {
                    timeout.value = setInterval(async () => {
                        timer.value--;
                        if (timer.value === 0) {
                            if (timeout.value)
                                clearInterval(timeout.value);
                        }
                    }, 1000);
                }
            });

            joinConnection.onclose(async () => {
                // connection.value = null;
                // messages.value = [];
                // users.value = [];
                // await sendMessage("Reconnected to server");
                console.log("DISCONNECTED FROM SERVER");
            });

            joinConnection.onreconnecting(() => {
                console.log("RECONNECTING TO SERVER");
            });

            joinConnection.onreconnected(() => {
                console.log("RECONNECTED TO SERVER");
                connection.value = joinConnection;

                if (cookieUser.value && cookieRoom.value) {
                    console.log("Reconnecting to group in store");
                    try {
                        const joinedRoom = tryJoinRoom();
                        if (!joinedRoom) {
                            navigateTo("/join");
                        }
                    } catch (error) {
                        console.log("Unable to connect in middleware:", error);
                    }
                }
            });

            await joinConnection.start();
            await joinConnection.invoke("JoinRoom", { user, room, userType });
            connection.value = joinConnection;

            if (joinConnection.state === HubConnectionState.Connected) {
                console.log("User connected");
                cookieUser.value = user;
                cookieRoom.value = room;
            }
        } catch (e) {
            console.log("HubConnection Error:", e);
        }
    };

    const sendCard = async (card: UNOCard): Promise<void> => {
        try {
            if (connection.value !== null) {
                await connection.value.invoke("SendCard", card);
            }
        } catch (e) {
            console.log(e);
        }
    };

    const sendColor = async (color: UnoColor): Promise<void> => {
        try {
            if (connection.value !== null) {
                await connection.value.invoke("SendColor", color);
            }
        } catch (e) {
            console.log(e);
        }
    };

    const drawCard = async (): Promise<void> => {
        if (connection.value === null) {
            return;
        }
        await connection.value.invoke("DrawCard");
    };

    const startGame = async (): Promise<void> => {
        if (connection.value === null) {
            return;
        }

        await connection.value.invoke("StartGame");
    };

    const sendMessage = async (message: string): Promise<void> => {
        try {
            if (connection.value !== null) {
                await connection.value.invoke("SendMessage", message);
            }
        } catch (e) {
            console.log(e);
        }
    };

    const closeConnection = async (): Promise<void> => {
        try {
            if (connection.value !== null) {
                await connection.value.stop();
            }
        } catch (e) {
            console.log(e);
        }
    };

    // Must return all state properties
    // https://pinia.vuejs.org/core-concepts/
    return {
        connection,
        isConnected,
        isPlayer,
        cards,
        messages,
        users,
        canSelectWildColor,
        user,
        room,
        cookieUser,
        cookieRoom,
        timer,
        tryCreateRoom,
        tryJoinRoom,
        sendCard,
        sendColor,
        drawCard,
        startGame,
        sendMessage,
        closeConnection
    };
});
