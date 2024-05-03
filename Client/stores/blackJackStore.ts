import { ref } from "vue";
import { defineStore } from "pinia";
import { useBaseStore } from "~/stores/baseStore";

export const useBlackJackStore = defineStore("blackjack", () => {
  // Debugging purposes
  const LOG_PREFIX = "Black - ";
  const log = function (...args: any[]) {
      const modifiedArgs = args.map(arg => `${ LOG_PREFIX }${ arg }`);
      console.log.apply(console, modifiedArgs);
  };
  const store = useBaseStore();
  const gameJson = ref<string>("");
  const { $api } = useNuxtApp();
  const gameType = ref<string>("");
  const players = ref<BlackJackPlayer[]>([]);
  const winners = ref<string[]>([]);
  const losers = ref<string[]>([]);
  const stalemates = ref<string[]>([]);
  const gameStarted = ref<boolean | null>(false);
  const allPlayersHaveBet = ref<boolean | null>(false);

  if (players.value != null) {
    console.log(players.value);
  }
  const currentPlayer = ref<string>("");

  const { gameConnection, messages, user, room } = storeToRefs(store);

  const drawBlackJackCard = async (): Promise<void> => {
    if (gameConnection.value === null) {
      return;
    }
    console.log("played a card");
    await gameConnection.value.invoke("DrawCardBlackJackHub");
  }
  
  const standBlackJackPlayer = async (): Promise<void> => {
    if (gameConnection.value === null) {
      return;
    }
    console.log("player has stood");
    await gameConnection.value.invoke("StandBlackJackHub");
  }

  const betBlackJackPlayer = async (bet: number): Promise<void> => {
    if (gameConnection.value === null) {
      return;
    }
    console.log("player has made bet");
    await gameConnection.value.invoke("betBlackJackHub", bet);
  }


  const registerHandlers = (): void => {
    if (gameConnection.value === null) return;
      gameConnection.value.on("ReceiveJSON", (blackJackJson: string) => {
        gameJson.value = blackJackJson;
        parseJson(gameJson.value);
    });
  }

  const parseJson = async (json: string) => {
      const parsed = JSON.parse(json);
      gameType.value = parsed.GameType;
      players.value = parsed.ActivePlayers;
      currentPlayer.value = parsed.CurrentPlayer;
      winners.value = parsed.Winners,
      losers.value = parsed.Losers,
      stalemates.value = parsed.Stalemates,
      allPlayersHaveBet.value = parsed.AllPlayersHaveBet
      gameStarted.value = parsed.GameStarted
  }

  return {
      betBlackJackPlayer,
      standBlackJackPlayer,
      drawBlackJackCard,
      parseJson,
      registerHandlers,
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