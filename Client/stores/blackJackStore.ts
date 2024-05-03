import { ref } from "vue";
import { defineStore } from "pinia";
import { useBaseStore } from "~/stores/baseStore";
import {
  HubConnectionState
} from "@microsoft/signalr";

export const useBlackJackStore = defineStore("blackjack", () => {
  // Debugging purposes
  const LOG_PREFIX = "Black - ";
  const log = function (...args: any[]) {
      const modifiedArgs = args.map(arg => `${ LOG_PREFIX }${ arg }`);
      console.log.apply(console, modifiedArgs);
  };

  const isGameConnected = computed<boolean>(() => gameConnection.value !== null && gameConnection.value.state === HubConnectionState.Connected);
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
  };

  const betBlackJackPlayer = async (bet: number): Promise<void> => {
    if (gameConnection.value === null) {
      return;
    }
    console.log("player has made bet");
    await gameConnection.value.invoke("betBlackJackHub", bet);
  };

  const pingblackjack = async (): Promise<void> => {
    if (!isGameConnected) return;
    await gameConnection.value?.invoke("PingBlackJack");
  };

  watch(gameConnection, async (newValue, oldValue) => {
    log("before watcher424242");
    // if (newValue === null) return;
    log("after watcher424242");

    registerHandlers();
  });

  const registerHandlers = (): void => {
    log("in registerhandlers");
    if (gameConnection.value === null) return;
      gameConnection.value.on("ReceiveJSON", (blackJackJson: string) => {
        gameJson.value = blackJackJson;
        parseJson(gameJson.value);
    });

    gameConnection.value.on("PongBlackJack", () => {
      log("Received pong from blackjack");
    });
  };

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
      pingblackjack,
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