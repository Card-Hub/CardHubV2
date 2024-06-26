import { ref } from "vue";
import { defineStore, storeToRefs } from "pinia";
//import { useWebSocketStore } from "./webSocketStore";


// noinspection DuplicatedCode

//import { defineStore } from "pinia";
import {
    type HubConnection,
    HubConnectionState
} from "@microsoft/signalr";
import { computed } from "vue";


export const useUneStore = defineStore("une", () => {
    // Debugging purposes
    const LOG_PREFIX = "UneHub - ";
    const log = function (...args: any[]) {
        const modifiedArgs = args.map(arg => `${ LOG_PREFIX }${ arg }`);
        console.log.apply(console, modifiedArgs);
    };

    // baseStore.ts
    const baseStore = useBaseStore();
    const { gameConnection, isPlayer } = storeToRefs(baseStore);

    const isGameConnected = computed<boolean>(() => gameConnection.value !== null && gameConnection.value.state === HubConnectionState.Connected);

    // watch(gameConnection, async (newValue, oldValue) => {
    //     if (newValue === null || !isGameConnected.value) return;
    //
    //     registerHandlersUne();
    // });

    const registerHandlersUne = (gameConnection: HubConnection): void => {
        if (gameConnection === null) return;

        //gameConnection.value.on("ReceiveCards", (cards: CahCard[]) => {
        //    log("Received cards", cards);
        //});
        gameConnection.on("Pong", () => {
            log("Received pong");
        });

        gameConnection.on("ReceiveJson", (json: string) => {
          log("Received json");
          parseJson(json);
      });
        gameConnection.on("ReceiveError", (errorMessage: string) => {
          toast.add({
                  severity: "contrast",
                  summary: errorMessage,
                  life: 5000
                });
        });
    };


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

    const ping = async (): Promise<void> => {
        if (!isGameConnected) return;
        await gameConnection.value?.invoke("Ping");
    };
    const startGame = async () : Promise<void> => {
      log("Clicked Start Game");
      await gameConnection.value?.invoke("StartGame");
    }

    //
    const drawCard = async() : Promise<void> => {
      log("Clicked Draw");
      await gameConnection.value?.invoke("DrawCard");
    }
    const pressUne = async() : Promise<void> => {
      log("Clicked Une");
      await gameConnection.value?.invoke("PressUne");
    }
    const selectWildColor = async(color: string) : Promise<void> => {
      log("Selected wild color ${color}");
      await gameConnection.value?.invoke("SelectWildColor", color);
    }
    const playCard = async(cardJson: string) : Promise<void> => {
      log("Drew wild color ${color}");
      await gameConnection.value?.invoke("PlayCard", cardJson);
    }

    //

    const gameType = ref<string>("");
    const gameStarted = ref<boolean>(false);
    const currentColor = ref<string>("");
    const direction = ref<string>("");
    const someoneNeedsToSelectColor = ref<boolean>(false);
    const playerWhoHasPrompt = ref<string>(""); // not implemented yet
    const winner = ref<string>("");
    const players = ref<unePlayer[]>([]);
    const canPressUne = ref<boolean>(false);
    const currentPlayer = ref<string>("");
    const discardPile = ref<UNOCard[]>([]);
    const deckAmt = ref<number>(0);
    ;
    const parseJson = async(json: string) : Promise<void> => {
      log("Parsing Json");
      log(json);
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
      console.log("players: ");
      console.log(players.value);
      players.value.forEach(player => {
        player.Hand.forEach(card => {
          console.log(card);
        });
      });
    }


    // Must return all state properties
    // https://pinia.vuejs.org/core-concepts/
    return {
        isGameConnected,
        closeConnection, sendAvatar, sendGameType, kickPlayer, registerHandlersUne, ping,

        startGame,
        parseJson,
        drawCard,
        selectWildColor,
        pressUne,
        playCard,
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
        canPressUne,
        deckAmt
    };
});


//export const useUneStore = defineStore("une", () => {
    //const store = useWebSocketStore();
    //const { gameJson, user } = storeToRefs(store);
    // variables for the store that will be called by the gameboard
    //const gameType = ref<string>("");
    //const gameStarted = ref<boolean>(false);
    //const currentColor = ref<string>("");
    //const direction = ref<string>("");
    //const someoneNeedsToSelectColor = ref<boolean>(false);
    //const playerWhoHasPrompt = ref<string>(""); // not implemented yet
    //const winner = ref<string>("");
    //const players = ref<unePlayer[]>([]);
    //const canPressUne = ref<boolean>(false);
    //if (players.value != null) {
    //    console.log("loggig players");
    //    console.log(players.value);
    //}
    //const currentPlayer = ref<string>("");
    //const discardPile = ref<UNOCard[]>([]);
    //const deckAmt = ref<number>(0);

    //const parseJson = async (json: string) => {
    //    const parsed = JSON.parse(json);
    //    gameType.value = parsed.GameType;
    //    gameStarted.value = parsed.GameStarted;
    //    currentColor.value = parsed.CurrentColor;
    //    direction.value = parsed.Direction;
    //    someoneNeedsToSelectColor.value = parsed.SomeoneNeedsToSelectWildColor;
    //    playerWhoHasPrompt.value = parsed.PlayerWhoHasUnoPrompt;
    //    winner.value = parsed.Winner;
    //    currentPlayer.value = parsed.CurrentPlayer;
    //    deckAmt.value = parsed.DeckCardCount;

    //    discardPile.value = parsed.DiscardedCards;
    //    players.value = parsed.ActivePlayers;
        //console.log("players: ");
        //console.log(players.value);
        //players.value.forEach(player => {
        //  player.Hand.forEach(card => {
        //    console.log(card);
        //  });
        //});

    //};

    //const getYourValues = () => {
    //  var youPlayer = null;
    //  // get your cards
    //  players.value.forEach(player => {
    //    if (player.name == user.value) {
    //      youPlayer = player;
    //      youPlayer.cards.forEach(card => {
    //        yourCards.value.push(card)
    //      });
    //    }
    //  });
    //} 

//    return {
//        parseJson,
//        gameType,
//        gameStarted,
//        currentColor,
//        direction,
//        someoneNeedsToSelectColor,
//        playerWhoHasPrompt,
//        winner,
//        players,
//        currentPlayer,
//        discardPile,
//        deckAmt
//    };
//});