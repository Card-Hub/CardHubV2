// noinspection DuplicatedCode

import { defineStore } from "pinia";
import {
    type HubConnection,
    HubConnectionBuilder,
    HubConnectionState,
    LogLevel
} from "@microsoft/signalr";
import { computed, ref } from "vue";
import { useRuntimeConfig } from "nuxt/app";
import { useNuxtApp } from "nuxt/app";

import { GameType } from "~/types";


interface PlayerMessage {
    name: string;
    message: string;
}

export const useBaseStore = defineStore("base", () => {
    // Debugging purposes
    const LOG_PREFIX = "BaseHub - ";
    const log = function(...args: any[]){
        const modifiedArgs = args.map(arg => `${ LOG_PREFIX }${ arg }`);
        console.log.apply(console, modifiedArgs);
    }

    const { $api } = useNuxtApp();

    enum UserType {
        Gameboard,
        Player
    }

    const baseConnection = ref<HubConnection | null>(null);
    const gameConnection = ref<HubConnection | null>(null);
    const isPlayer = ref<boolean | null>(null);
    const isConnected = computed(() => baseConnection.value !== null && baseConnection.value.state === HubConnectionState.Connected);
    const gameType = ref<GameType | null>(null);

    const messages = ref<PlayerMessage[]>([]);
    const users = ref<Array<LobbyUser>>([]);

    const user = ref("");
    const room = ref("");

    const runtimeConfig = useRuntimeConfig();

    // Attempt to create a new room for the Gameboard device
    const tryConnectGameboard = async (gameType: GameType): Promise<string | null> => {
        try {
            const response = await $api<string>("Game/CreateRoom",
                {
                    method: "POST",
                    body: JSON.stringify(gameType) // TODO: Fix this
                });
            if (!response) return null;

            const connection: BaseConnection = { room: response };
            await joinRoom(connection);
            isPlayer.value = false;
            room.value = response;

            log("Room created: ", room.value);
            return room.value
        } catch (e) {
            log("Failed to create room", e);
        }
        return null;
    };

    // Attempt to join an existing room as a Player device
    const tryConnectPlayer = async (user: string, room: string): Promise<GameType | null> => {
        try {
            const gameType = await $api<GameType>(`game/verifycode/${ room }`, { method: "GET" });
            if (!gameType) {
                log("Invalid room code");
                return null;
            }

            const connection: BaseConnection = { name: user, room: room };
            await joinRoom(connection);
            isPlayer.value = true;
            return gameType;
        } catch (e) {
            log(e);

        }

        return null;
    };

    const joinRoom = async (connection: BaseConnection): Promise<void> => {
        const webSocketUrl = `${ runtimeConfig.public.baseURL }/${ runtimeConfig.public.baseHub }`;
        try {
            // HubConnection configuration
            // https://learn.microsoft.com/en-us/aspnet/core/signalr/configuration?view=aspnetcore-8.0&tabs=dotnet#configure-client-options
            const joinConnection = new HubConnectionBuilder()
                .withUrl(webSocketUrl)
                .withStatefulReconnect()
                .withAutomaticReconnect()
                .configureLogging(LogLevel.Information)
                .build();

            joinConnection.on("ReceiveMessage", (playerMessage: PlayerMessage) => {
                messages.value.push(playerMessage);
                log(playerMessage);
            });

            joinConnection.on("ReceiveAvatars", (avatars: Array<LobbyUser>) => {
                users.value = avatars;
            });

            joinConnection.on("Log", (string: string) => {
                log("Backend logged:", string);
                log(string);
            });


            joinConnection.onclose(async () => {
                log("DISCONNECTED FROM SERVER");
            });

            joinConnection.onreconnecting(() => {
                log("RECONNECTING TO SERVER");
            });

            joinConnection.onreconnected(() => {
                log("RECONNECTED TO SERVER");
                baseConnection.value = joinConnection;
            });


            await joinConnection.start();
            await joinConnection.invoke("JoinRoom", connection);
            baseConnection.value = joinConnection;

            if (joinConnection.state === HubConnectionState.Connected) {
                log("User connected");
            }
        } catch (e) {
            log("joinRoom error:\n", e);
        }
    };

    const sendAvatar = async (avatar: string): Promise<void> => {
        if (baseConnection.value === null) return;
        await baseConnection.value.invoke("SendAvatar", avatar);
    };

    const sendMessage = async (message: string): Promise<void> => {
        if (baseConnection.value === null) return;
        await baseConnection.value.invoke("SendMessage", message);
    };

    const closeConnection = async (): Promise<void> => {
        if (baseConnection.value === null) return;
        await baseConnection.value.stop();
    };

    const kickPlayer = async (player: string): Promise<void> => {
        if (baseConnection.value === null) return;
        await baseConnection.value.invoke("KickPlayer", player);
    };

    // Must return all state properties
    // https://pinia.vuejs.org/core-concepts/
    return {
        baseConnection, isConnected, isPlayer, messages, users, user, room,
        tryConnectGameboard, tryConnectPlayer, sendMessage, closeConnection, sendAvatar, kickPlayer
    };
});
