// noinspection DuplicatedCode

import { defineStore } from "pinia";
import {
    type HubConnection,
    HubConnectionState
} from "@microsoft/signalr";
import { computed } from "vue";
import { useBaseStore } from "~/stores/baseStore";


export const useCahStore = defineStore("cah", () => {
    // Debugging purposes
    const LOG_PREFIX = "CahHub - ";
    const log = function (...args: any[]) {
        const modifiedArgs = args.map(arg => `${ LOG_PREFIX }${ arg }`);
        console.log.apply(console, modifiedArgs);
    };

    // baseStore.ts
    const baseStore = useBaseStore();
    const { gameConnection, isPlayer, gameType } = storeToRefs(baseStore);

    const gameStarted = ref<boolean>(false);
    const isGameConnected = computed<boolean>(() => gameConnection.value !== null && gameConnection.value.state === HubConnectionState.Connected);


    function registerHandlersCah(gameConnection: HubConnection): void {
        if (gameConnection === null) return;
        console.log("Registering Cah handlers");
        gameConnection.on("ReceiveCards", (cards: CahCard[]) => {
            log("Received cards", cards);
        });

        gameConnection.on("GameStarted", () => {
            gameStarted.value = true;
        });

        gameConnection.on("Pong", () => {
            log("Received pong");
        });
    }

    const startGame = async (): Promise<void> => {
        if (!isGameConnected) return;
        if (gameStarted.value) return;

        await gameConnection.value?.invoke("StartGame");
    }

    const ping = async (): Promise<void> => {
        if (!isGameConnected) return;
        await gameConnection.value?.invoke("Ping");
    };


    // Must return all state properties
    // https://pinia.vuejs.org/core-concepts/
    return {
        isGameConnected, gameStarted,
        registerHandlersCah, ping, startGame
    };
});
