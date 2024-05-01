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

    interface LobbyUser {
      Name: string
      Avatar: string
    }
}