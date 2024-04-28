import { defineStore, storeToRefs } from "pinia";
import {
    // HttpTransportType,
    type HubConnection,
    HubConnectionBuilder,
    HubConnectionState,
    LogLevel
} from "@microsoft/signalr";
import { computed, ref } from "vue";
import { useNuxtApp, useRuntimeConfig } from "nuxt/app";


export const useCahStore = defineStore("cah", () => {
    const runtimeConfig = useRuntimeConfig();
    const { $api } = useNuxtApp();
    enum UserType {
        Gameboard,
        Player
    }

    const user = ref<string | null>(null);
    const room = ref<string | null>(null);

    const timer = ref<number>(0);
    const timeout = ref<NodeJS.Timeout | null>(null);

    const tryCreateRoom = async (): Promise<boolean> => {
        try {
            room.value = await $api<string>("Game/CreateRoom", { method: "POST" });
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


            joinConnection.on("ReceiveCard", (fromUser: string, card: UNOCard) => {
                console.log(card);
            });

            joinConnection.on("StartTimer", async (time: number) => {
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
                console.log("DISCONNECTED FROM SERVER");
            });

            joinConnection.onreconnecting(() => {
                console.log("RECONNECTING TO SERVER");
            });

            joinConnection.onreconnected(() => {
                console.log("RECONNECTED TO SERVER");
            });

            await joinConnection.start();
            await joinConnection.invoke("JoinRoom", { user, room, userType });
            connection.value = joinConnection;

            if (joinConnection.state === HubConnectionState.Connected) {
                console.log("User connected");
            }
        } catch (e) {
            console.log("HubConnection Error:", e);
        }
    };



    // Must return all state properties
    // https://pinia.vuejs.org/core-concepts/
    return {

    };
});
