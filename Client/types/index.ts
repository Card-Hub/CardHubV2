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
        Afk: boolean
        //cards: Card[]
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
    }

    interface LobbyUser {
      Name: string
      Avatar: string
    }
}