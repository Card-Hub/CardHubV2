import { GameType } from "~/types";

export default defineNuxtPlugin(() => {
    const instance = (gameType: GameType): string => {
        switch (gameType) {
            case GameType.BlackJack:
                return "blackjack";
            case GameType.Cah:
                return "cah";
            case GameType.Poker:
                return "poker";
            case GameType.Une:
                return "une";
            default:
                throw new Error("Invalid game type");
        }
    }

    return {
        provide: {
            gameToString: instance
        }
    }
})