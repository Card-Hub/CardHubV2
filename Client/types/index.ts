export {};

declare global {

    interface Card {
        Id: number;
        Value: string;
    }

    interface StandardCard extends Card {
        Suit: string;
    }

    interface UNOCard extends Card {
        Color: string;
    }

    interface UserMessage {
        user: string;
        message: string;
    }

    interface Player {
        Name: string;
        Avatar: string;
        Afk: boolean;
        //cards: Card[]
    }

    interface unePlayer extends Player {
        PickingWildColor: boolean;
        Hand: UNOCard[];
    }

    interface CahCard {
        text: string;
        type: CahType;
    }

    interface LobbyUser {
        name: string;
        Avatar: string;
    }

    interface ConnectionOptions {
        room: string;
        name?: string;
    }

    interface BasePlayer {
        name: string;
        avatar: string;
    }
}

export const enum CahType {
    White,
    Black
}

export enum GameType {
    BlackJack,
    Cah,
    Poker,
    Une
}