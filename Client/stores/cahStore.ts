// noinspection DuplicatedCode

import { defineStore } from "pinia";
import {
    type HubConnection,
    HubConnectionState
} from "@microsoft/signalr";
import { computed, type Ref } from "vue";
import { GameType } from "~/types";
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

    const isGameConnected = computed<boolean>(() => gameConnection.value !== null && gameConnection.value.state === HubConnectionState.Connected);

    // watch(gameConnection, async (newValue, oldValue) => {
    //     if (gameType.value !== GameType.Cah) return;
    //     if (newValue === null) return;
    //
    //     registerHandlers();
    // });

    function registerHandlersCah(gameConnection: HubConnection): void {
        if (gameConnection === null) return;
        console.log("Registering Cah handlers");
        gameConnection.on("ReceiveCards", (cards: CahCard[]) => {
            log("Received cards", cards);
        });

        // gameConnection.value.on()

        gameConnection.on("Pong", () => {
            log("Received pong");
        });
    }

    const closeConnection = async (): Promise<void> => {
        if (!isGameConnected) return;
        await gameConnection.value?.stop();
    };

    const ping = async (): Promise<void> => {
        if (!isGameConnected) return;
        await gameConnection.value?.invoke("Ping");
    };


    // Must return all state properties
    // https://pinia.vuejs.org/core-concepts/
    return {
        isGameConnected,
        closeConnection, registerHandlersCah, ping
    };
});
