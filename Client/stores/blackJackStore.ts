import { ref } from "vue";
import { defineStore } from "pinia";
import { useBaseStore } from "~/stores/baseStore";
import {
    type HubConnection,
    HubConnectionState
} from "@microsoft/signalr";

export const useBlackJackStore = defineStore("blackjack", () => {
    // Debugging purposes
    const LOG_PREFIX = "Black - ";
    const log = function (...args: any[]) {
        const modifiedArgs = args.map(arg => `${ LOG_PREFIX }${ arg }`);
        console.log.apply(console, modifiedArgs);
    };

    const { $api } = useNuxtApp();

    const store = useBaseStore();
    const { gameConnection, messages, user, room } = storeToRefs(store);

    const isGameConnected = computed<boolean>(() => gameConnection.value !== null && gameConnection.value.state === HubConnectionState.Connected);
    const gameJson = ref<string>("");
    const gameType = ref<string>("");
    const players = ref<BlackJackPlayer[]>([]);
    const winners = ref<string[]>([]);
    const losers = ref<string[]>([]);
    const stalemates = ref<string[]>([]);
    const gameStarted = ref<boolean>(false);
    const allPlayersHaveBet = ref<boolean | null>(false);
    const currentPlayer = ref<string>("");


    const registerHandlersBlackJack = (gameConnection: HubConnection): void => {
        if (gameConnection === null) return;

        gameConnection.on("ReceiveJSON", (blackJackJson: string) => {
            gameJson.value = blackJackJson;
            parseJson(gameJson.value);
        });

        gameConnection.on("Pong", () => {
            log("Received pong from blackjack");
        });

        gameConnection.on("GameStarted", () => {
            gameStarted.value = true;
        });
        gameConnection.on("ReceiveJson", (json: string) => {
            parseJson(json);
            log(json);
        });
    };

    const startGame = async (): Promise<void> => {
        if (!isGameConnected) return;
        if (gameStarted.value) return;
        
        await gameConnection.value?.invoke("StartGame");
    }

    const drawBlackJackCard = async (): Promise<void> => {
        if (gameConnection.value === null) return;

        console.log("played a card");
        await gameConnection.value.invoke("DrawCardBlackJackHub");
    };

    const standBlackJackPlayer = async (): Promise<void> => {
        if (gameConnection.value === null) return;

        console.log("player has stood");
        await gameConnection.value.invoke("StandBlackJackHub");
    };

    const betBlackJackPlayer = async (bet: number): Promise<void> => {
        if (gameConnection.value === null) return;

        console.log("player has made bet");
        await gameConnection.value.invoke("BetBlackJackHub", bet);
    };

    const ping = async (): Promise<void> => {
        if (!isGameConnected) return;

        await gameConnection.value?.invoke("Ping");
    };

    const parseJson = async (json: string) => {
        const parsed = JSON.parse(json);
        gameType.value = parsed.GameType;
        players.value = parsed.Players;
        currentPlayer.value = parsed.CurrentPlayer;
        winners.value = parsed.Winners;
        losers.value = parsed.Losers;
        stalemates.value = parsed.Stalemates;
        allPlayersHaveBet.value = parsed.AllPlayersHaveBet;
        gameStarted.value = parsed.GameStarted;
    };

    return {
        betBlackJackPlayer,
        standBlackJackPlayer,
        drawBlackJackCard,
        parseJson,
        registerHandlersBlackJack,
        ping,
        startGame,
        gameType,
        players,
        currentPlayer,
        winners,
        losers,
        stalemates,
        allPlayersHaveBet,
        gameStarted,
        user
    };
});