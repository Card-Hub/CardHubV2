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
        id: number;
        text: string;
        type: CahType;
    }

    interface LobbyUser {
        name: string;
        Avatar: string;
    }

    interface BaseConnection {
        room: string;
        name?: string;
    }
}

export const enum CahType {
    White,
    Black
}

export const enum GameType {
    BlackJack,
    Cah,
    Poker,
    Une
}