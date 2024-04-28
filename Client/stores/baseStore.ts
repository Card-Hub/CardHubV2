// noinspection DuplicatedCode

import { defineStore } from "pinia";
import {
    // HttpTransportType,
    type HubConnection,
    HubConnectionBuilder,
    HubConnectionState,
    LogLevel
} from "@microsoft/signalr";
import { computed, ref } from "vue";
import { useRuntimeConfig } from "nuxt/app";
import { useNuxtApp } from "nuxt/app";
import { GameType } from "~/types";

export const useBaseStore = defineStore("base", () => {
    const { $api } = useNuxtApp();

    enum UserType {
        Gameboard,
        Player
    }

    const connection = ref<HubConnection | null>(null);
    const gameType = ref<GameType | null>(null);
    const isPlayer = ref<boolean | null>(null);
    const isConnected = computed(() => connection.value !== null && connection.value.state === HubConnectionState.Connected);

    const messages = ref<UserMessage[]>([]);
    const users = ref<string[]>([]);

    const user = ref("");
    const room = ref("");

    const runtimeConfig = useRuntimeConfig();

    const lobbyUsers = ref<Array<LobbyUser>>([]);


    // Attempt to create a new room for the Gameboard device
    const tryCreateRoom = async (gameType: GameType): Promise<boolean> => {
        try {
            const response = await $api<string>("Game/CreateRoom",
                {
                    method: "POST",
                    body: JSON.stringify(gameType)
                });
            if (!response) return false;

            room.value = response;
            await joinRoom(user.value, room.value);
            isPlayer.value = false;
            console.log("Room created: ", room.value);
            return true;
        } catch (e) {
            console.log("Failed to create room", e);
        }
        return false;
    };

    // Attempt to join an existing room as a Player device
    const tryJoinRoom = async (userName: string, roomId: string): Promise<boolean> => {
        try {
            const roomCodeExists = await $api<boolean>(`game/verifycode/${ roomId }`, { method: "GET" });
            if (!roomCodeExists) {
                console.log("Invalid room code");
                return false;
            }

            await joinRoom(userName, roomId);
            isPlayer.value = true;
            return true;
        } catch (e) {
            console.log(e);
        }

        console.log("Failed to join room");
        return false;
    };

    const joinRoom = async (user: string, room: string): Promise<void> => {
        const webSocketUrl = `${ runtimeConfig.public.baseURL }/${ gameType.value }`;
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

            joinConnection.on("UsersInRoom", (groupUsers: string[]) => {
                users.value = groupUsers;
            });

            joinConnection.on("ReceiveAvatars", (json: string) => {
                let jsonLobbyUsers = JSON.parse(json);
                lobbyUsers.value.length = 0;
                for (let lobbyUserIndex in jsonLobbyUsers) {
                    lobbyUsers.value.push(jsonLobbyUsers[lobbyUserIndex]);
                }

                console.log(lobbyUsers.value);
            });

            joinConnection.on("Log", (string: string) => {
                console.log("Backend logged:", string);
                console.log(string);
            });


            joinConnection.onclose(async () => {
                console.log("BaseHub - DISCONNECTED FROM SERVER");
            });

            joinConnection.onreconnecting(() => {
                console.log("BaseHub - RECONNECTING TO SERVER");
            });

            joinConnection.onreconnected(() => {
                console.log("BaseHub - RECONNECTED TO SERVER");
                connection.value = joinConnection;
            });


            await joinConnection.start();
            await joinConnection.invoke("JoinRoom", { user, room });
            connection.value = joinConnection;

            if (joinConnection.state === HubConnectionState.Connected) {
                console.log("User connected");
            }
        } catch (e) {
            console.log("BaseHub - joinRoom error:\n", e);
        }
    };

    const sendAvatar = async (avatar: string): Promise<void> => {
        try {
            if (connection.value !== null) {
                await connection.value.invoke("SendAvatar", avatar);
            }
        } catch (e) {
            console.log(e);
        }
    };

    const sendGameType = async (gameType: string): Promise<void> => {
        try {
            if (connection.value !== null) {
                await connection.value.invoke("SendGameType", gameType);
            }
        } catch (e) {
            console.log(e);
        }
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

    const kickPlayer = async (player: string): Promise<void> => {
        try {
            if (connection.value !== null) {
                await connection.value.invoke("KickPlayer", player);
            }
        } catch (e) {
            console.log(e);
        }
    };

    // Must return all state properties
    // https://pinia.vuejs.org/core-concepts/
    return {
        connection, isConnected, isPlayer, messages, users, user, room, lobbyUsers,
        tryCreateRoom, tryJoinRoom, sendMessage, closeConnection, sendAvatar, sendGameType, kickPlayer
    };
});
