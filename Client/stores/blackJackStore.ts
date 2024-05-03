import { ref } from "vue";
import { defineStore } from "pinia";
import { useWebSocketStore } from "~/stores/webSocketStore";




export const useBlackJackStore = defineStore("blackjack", () => {
  const store = useWebSocketStore();
  const { gameJson, user } = storeToRefs(store);
  const { $api } = useNuxtApp();
  const gameType = ref<string>("");
  const gameStarted = ref<boolean>(false);
  const playerWhoHasPrompt = ref<string>(""); // not implemented yet
  const winner = ref<string>("");
  const players = ref<unePlayer[]>([]);
  const winners = ref<string[]>([]);
  const losers = ref<string[]>([]);
  const stalemates = ref<string[]>([]);
  if (players.value != null) {
    console.log("loggig players");
    console.log(players.value);
  }
  const currentPlayer = ref<string>("");
  const discardPile = ref<UNOCard[]>([]);
  const deckAmt = ref<number>(0);

  const { connection, isConnected, messages, room } = storeToRefs(store);

  const drawBlackJackCard = async (): Promise<void> => {
    if (connection.value === null) {
      return;
    }
    console.log("played a card");
    await connection.value.invoke("DrawCardBlackJackHub");
  }


  const standBlackJackPlayer = async (): Promise<void> => {
    if (connection.value === null) {
      return;
    }
    console.log("player has stood");
    await connection.value.invoke("StandBlackJackHub");
  }


  const betBlackJackPlayer = async (bet: number): Promise<void> => {
    if (connection.value === null) {
      return;
    }
    console.log("player has made bet");
    await connection.value.invoke("betBlackJackHub", bet);
  }

  const parseJson = async (json: string) => {
      const parsed = JSON.parse(json);
      gameType.value = parsed.GameType;
      players.value = parsed.ActivePlayers;
      currentPlayer.value = parsed.CurrentPlayer;
      winners.value = parsed.Winners,
      losers.value = parsed.Losers,
      stalemates.value = parsed.Stalemates 
  }

  return {
      betBlackJackPlayer,
      standBlackJackPlayer,
      drawBlackJackCard,
      parseJson,
      gameType,
      players,
      currentPlayer,
      winners,
      losers,
      stalemates
  };
});