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
import type { GameType } from "~/types";


export const useCahStore = defineStore("cah", () => {
    // Debugging purposes
    const LOG_PREFIX = "CahHub - ";
    const log = function(...args: any[]){
        const modifiedArgs = args.map(arg => `${ LOG_PREFIX }${ arg }`);
        console.log.apply(console, modifiedArgs);
    }

    const { $api } = useNuxtApp();
    const { $gameToHubString } = useNuxtApp();

    const cahConnection = ref<HubConnection | null>(null);
    const isConnected = computed(() => cahConnection.value !== null && cahConnection.value.state === HubConnectionState.Connected);

    const runtimeConfig = useRuntimeConfig();


    const tryConnectGameboard = async (roomId: string, gameType: GameType): Promise<boolean> => {
        const connection: BaseConnection = { room: roomId };
        if (!await joinRoom(connection, gameType)) {
            log("Failed to connect gameboard");
            return false;
        }

        log("Room created: ", roomId);
        return true;
    };

    const tryConnectPlayer = async (name: string, roomId: string): Promise<boolean> => {
        try {
            const gameType = await $api<GameType>(`game/verifycode/${ roomId }`, { method: "GET" });
            if (!gameType) {
                log("Invalid room code");
                return false;
            }

            const connection: BaseConnection = { name: name, room: roomId };
            if (!await joinRoom(connection, gameType)) return false;

            log("Joined room: ", roomId);
            return true;
        } catch (e) {
            log("CahHub - ", e);
            return false;
        }
    };

    const joinRoom = async (connection: BaseConnection, gameType: GameType): Promise<boolean> => {
        const webSocketUrl = `${ runtimeConfig.public.baseURL }/${ $gameToHubString(gameType) }`;
        try {
            const joinConnection = new HubConnectionBuilder()
                .withUrl(webSocketUrl)
                .withStatefulReconnect()
                .withAutomaticReconnect()
                .configureLogging(LogLevel.Information)
                .build();

            joinConnection.on("Log", (string: string) => {
                log("[BACKEND LOG] ", string);
            });


            joinConnection.onclose(async () => {
                log("DISCONNECTED FROM SERVER");
            });

            joinConnection.onreconnecting(() => {
                log("RECONNECTING TO SERVER");
            });

            joinConnection.onreconnected(() => {
                log("RECONNECTED TO SERVER");
                cahConnection.value = joinConnection;
            });

            await joinConnection.start();
            await joinConnection.invoke("JoinRoom", connection);
            cahConnection.value = joinConnection;

            if (joinConnection.state === HubConnectionState.Connected) {
                log("User connected");
            }
            return true;

        } catch (e) {
            log("joinRoom error:\n", e);
            return false;
        }
    };


    const sendAvatar = async (avatar: string): Promise<void> => {
        if (cahConnection.value === null) return;
        await cahConnection.value.invoke("SendAvatar", avatar);
    };

    const sendGameType = async (gameType: string): Promise<void> => {
        if (cahConnection.value === null) return;
        await cahConnection.value.invoke("SendGameType", gameType);
    };

    const closeConnection = async (): Promise<void> => {
        if (cahConnection.value === null) return;
        await cahConnection.value.stop();
    };

    const kickPlayer = async (player: string): Promise<void> => {
        if (cahConnection.value === null) return;
        await cahConnection.value.invoke("KickPlayer", player);
    };


    // Must return all state properties
    // https://pinia.vuejs.org/core-concepts/
    return {
        cahConnection, isConnected,
        tryConnectCahGameboard: tryConnectGameboard, tryConnectCahPlayer: tryConnectPlayer, closeConnection, sendAvatar, sendGameType, kickPlayer
    };
});
