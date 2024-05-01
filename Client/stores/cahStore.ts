// noinspection DuplicatedCode

import { defineStore } from "pinia";
import {
    HubConnectionState
} from "@microsoft/signalr";
import { computed, } from "vue";


export const useCahStore = defineStore("cah", () => {
    // Debugging purposes
    const LOG_PREFIX = "CahHub - ";
    const log = function(...args: any[]){
        const modifiedArgs = args.map(arg => `${ LOG_PREFIX }${ arg }`);
        console.log.apply(console, modifiedArgs);
    }

    // baseStore.ts
    const baseStore = useBaseStore();
    const { gameConnection, isPlayer } = storeToRefs(baseStore);

    const isGameConnected = computed<boolean>(() => gameConnection.value !== null && gameConnection.value.state === HubConnectionState.Connected);

    // watch(gameConnection, async (newValue, oldValue) => {
    //     if (newValue === null || oldValue !== null || !isGameConnected.value) return;
    // });

    const registerHandlers = (): void => {
        gameConnection.value?.on("ReceiveCards", (cards: CahCard[]) => {
            log("Received cards", cards)
        });
        log("Registered handlers");
    }


    const sendAvatar = async (avatar: string): Promise<void> => {
        if (!isGameConnected) return;
        await gameConnection.value?.invoke("SendAvatar", avatar);
    };

    const sendGameType = async (gameType: string): Promise<void> => {
        if (!isGameConnected) return;
        await gameConnection.value?.invoke("SendGameType", gameType);
    };

    const closeConnection = async (): Promise<void> => {
        if (!isGameConnected) return;
        await gameConnection.value?.stop();
    };

    const kickPlayer = async (player: string): Promise<void> => {
        if (!isGameConnected) return;
        await gameConnection.value?.invoke("KickPlayer", player);
    };


    // Must return all state properties
    // https://pinia.vuejs.org/core-concepts/
    return {
        isGameConnected,
        closeConnection, sendAvatar, sendGameType, kickPlayer, registerHandlers
    };
});
