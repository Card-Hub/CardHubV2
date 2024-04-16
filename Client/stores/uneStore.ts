import { ref } from "vue";
import { defineStore } from "pinia";
import {useWebSocketStore} from "~/stores/webSocketStore";

const store = useWebSocketStore();
const { gameJson } = storeToRefs(store);

export const useUneStore = defineStore("une", () => {
    const { $api } = useNuxtApp();
    // variables for the store that will be called by the gameboard
    const gameType = ref<string>("");
    const gameStarted = ref<boolean>(false);
    const currentColor = ref<string>("");
    const direction = ref<string>("");
    const someoneNeedsToSelectColor = ref<boolean>(false);
    const playerWhoHasPrompt = ref<string>(""); // not implemented yet
    const winner = ref<string>("");
    const players = ref<unePlayer[]>([]);
    const currentPlayer = ref<string>("");
    const discardPile = ref<UNOCard[]>([]);
    const deckAmt = ref<number>(0);

    parseJson(gameJson.value);

    function parseJson(json: string) {
        const parsed = JSON.parse(json);
        gameType.value = parsed.GameType;
        gameStarted.value = parsed.GameStarted;
        currentColor.value = parsed.CurrentColor;
        direction.value = parsed.Direction;
        someoneNeedsToSelectColor.value = parsed.SomeoneNeedsToSelectWildColor;
        playerWhoHasPrompt.value = parsed.PlayerWhoHasUnoPrompt;
        winner.value = parsed.Winner;
        currentPlayer.value = parsed.CurrentPlayer;
        deckAmt.value = parsed.DeckCardCount;

        discardPile.value = parsed.DiscardedCards;
        players.value = parsed.ActivePlayers;
    }

    return {
        gameType,
        gameStarted,
        currentColor,
        direction,
        someoneNeedsToSelectColor,
        playerWhoHasPrompt,
        winner,
        players,
        currentPlayer,
        discardPile,
        deckAmt,
    };
});