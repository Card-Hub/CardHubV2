export { };

declare global {
    
    interface Card {
        Id: number
        Value: string
    }
    
    interface StandardCard extends Card {
        Suit: string
    }
    
    interface UNOCard extends Card {
        Color: string
    }

    interface UserMessage {
        user: string
        message: string
    }
    
    interface Player {
        Name: string
        Avatar: string
        Afk?: boolean
        //cards: Card[]
    }
    
    interface CahPlayer extends Player {
        Hand: CahCard[]
        Score: number
        IsCzar: boolean
        IsWinner: boolean
    }
    
    interface unePlayer extends Player {
        PickingWildColor: boolean
        Hand: UNOCard[]
        CanPressUne: boolean
    }
    
    interface PokerPlayer extends Player {
        CanFold: boolean
        CanCall: boolean
        CanRaise: boolean
        CanCheck: boolean
        Hand: StandardCard[]
        BestHand: string
        Folded: boolean
    }
    
    interface BlackJackPlayer extends Player {
        strConn: string
        Hand: StandardCard[]
        CurrentScore: number
        TotalMoney: number
        CurrentBet: number
        HasBet: boolean
        NotPlaying: boolean
        Busted: boolean
        Winner: boolean
        StillPlaying: boolean
        Standing: boolean
        Name: string;
    }

    interface CahCard {
        text: string;
        type: CahType;
    }

    interface LobbyUser {
        Name: string;
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

export enum CahType {
    White,
    Black
}

export enum GameType {
    BlackJack,
    Cah,
    Poker,
    Une
}