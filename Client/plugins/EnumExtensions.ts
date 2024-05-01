import { GameType } from "~/types";

export default defineNuxtPlugin(() => {
    const instance = (gameType: GameType): string => {
        switch (gameType) {
            case GameType.BlackJack:
                return "blackjackhub";
            case GameType.Cah:
                return "cahhub";
            case GameType.Poker:
                return "pokerhub";
            case GameType.Une:
                return "unehub";
            default:
                throw new Error("Invalid game type");
        }
    }

    return {
        provide: {
            gameToHubString: instance
        }
    }
})