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
    const log = function (...args: any[]) {
        const modifiedArgs = args.map(arg => `${ LOG_PREFIX }${ arg }`);
        console.log.apply(console, modifiedArgs);
    };

    const { $api } = useNuxtApp();
    const { $gameToString } = useNuxtApp();

    const baseConnection = ref<HubConnection | null>(null);
    const gameConnection = ref<HubConnection | null>(null);
    const isPlayer = ref<boolean | null>(null);
    const isBaseConnected = computed<boolean>(() => baseConnection.value !== null && baseConnection.value.state === HubConnectionState.Connected);
    const currentAvatar = computed<string | null>(() => {
        if (!isPlayer.value) return null;
        const currentPlayer = users.value.find(p => p.name === user.value);
        return currentPlayer?.avatar ?? null;
    });

    const messages = ref<PlayerMessage[]>([]);
    const users = ref<Array<BasePlayer>>([]);

    const user = ref("");
    const room = ref("");

    const runtimeConfig = useRuntimeConfig();

    const tryConnectGameboard = async (gameType: GameType, callback: () => void): Promise<boolean> => {
        try {
            const response = await $api<string>("Game/CreateRoom",
                {
                    method: "POST",
                    body: JSON.stringify(gameType)
                });
            if (!response) return false;

            const options: ConnectionOptions = { room: response };
            const baseConnect = await joinRoom(options, gameType);
            const gameConnect = await joinRoom(options, gameType, callback);

            if (!baseConnect || !gameConnect) {
                log("Failed to connect to server");
                console.log("Base: ", baseConnect, "Game: ", gameConnect)
                return false;
            }

            isPlayer.value = false;
            room.value = response;

            log("Room created: ", response);
            return true;
        } catch (e) {
            log("Error in TryConnectGameboard", e);
            return false;
        }
    };

    const tryConnectPlayer = async (user: string, room: string): Promise<GameType | null> => {
        try {
            const gameType = await $api<GameType>(`game/verifycode/${ room }`, { method: "GET" });
            if (gameType === null || gameType === undefined) {
                log(`Invalid room code (gametype): ${ gameType }`);
                return null;
            }

            const options: ConnectionOptions = { name: user, room: room };
            const baseConnect = await joinRoom(options, gameType);
            const gameConnect = await joinRoom(options, gameType, function () {});

            if (!baseConnect || !gameConnect) {
                log("Failed to connect to server");
                console.log("Base: ", baseConnect, "Game: ", gameConnect)
                return null;
            }

            isPlayer.value = true;
            return gameType;
        } catch (e) {
            log("Error in TryConnectPlayer", e);
            return null;
        }
    };

    const joinRoom = async (options: ConnectionOptions, gameType: GameType, callback?: () => void): Promise<boolean> => {
        try {
            const isBase = callback === undefined || callback === null;
            // if (!isBase && !isBaseConnected.value) return false; // shit breaks

            const hubPath = isBase ? runtimeConfig.public.baseHub : $gameToString(gameType) + "hub";
            const webSocketUrl = `${ runtimeConfig.public.baseURL }/${ hubPath }`;

            // HubConnection configuration
            // https://learn.microsoft.com/en-us/aspnet/core/signalr/configuration?view=aspnetcore-8.0&tabs=dotnet#configure-client-options
            const joinConnection = new HubConnectionBuilder()
                .withUrl(webSocketUrl)
                .withStatefulReconnect()
                .withAutomaticReconnect()
                .configureLogging(LogLevel.Warning)
                .build();

            if (isBase) {
                baseConnection.value = joinConnection;
                registerBaseHandlers();
            } else {
                callback?.();
            }

            joinConnection.onclose(() => {
                log("DISCONNECTED FROM SERVER");
            });

            joinConnection.onreconnecting(() => {
                log("RECONNECTING TO SERVER");
            });

            joinConnection.onreconnected(() => {
                log("RECONNECTED TO SERVER");
            });

            await joinConnection.start();
            await joinConnection.invoke("JoinRoom", options);

            if (joinConnection.state === HubConnectionState.Connected) {
                console.log("User connected");

                if (isBase) {
                    baseConnection.value = joinConnection;
                } else {
                    gameConnection.value = joinConnection;
                }
            }
            return true;
        } catch (e) {
            log("Error in joinRoom", e);
            return false;
        }
    };

    const registerBaseHandlers = (): void => {
        if (baseConnection.value === null) return;

        baseConnection.value.on("ReceiveMessage", (userMessage: PlayerMessage) => {
            messages.value.push(userMessage);
            console.log(userMessage);
        });

        baseConnection.value.on("ReceiveAvatars", (usersList: Array<BasePlayer>) => {
            users.value = usersList;
            console.log(usersList);
        });
    };


    const sendAvatar = async (avatar: string): Promise<void> => {
        if (baseConnection.value === null) return;
        await baseConnection.value.invoke("SendAvatar", avatar);
    };

    const sendMessage = async (message: string): Promise<void> => {
        if (baseConnection.value === null) return;
        await baseConnection.value.invoke("SendMessage", message);
    };

    const kickPlayer = async (player: string): Promise<void> => {
        if (baseConnection.value === null) return;
        await baseConnection.value.invoke("KickPlayer", player);
    };

    const closeConnection = async (): Promise<void> => {
        if (baseConnection.value === null) return;
        await baseConnection.value.stop();
    };

    // Must return all state properties
    // https://pinia.vuejs.org/core-concepts/
    return {
        baseConnection, gameConnection, isBaseConnected, isPlayer, messages, users, user, room, currentAvatar,
        tryConnectGameboard, tryConnectPlayer, sendMessage, closeConnection, sendAvatar, kickPlayer
    };
});
