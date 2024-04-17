export { };

declare global {
    
    interface Card {
        id: number
        value: string
    }
    
    interface StandardCard extends Card {
        suit: string
    }
    
    interface UNOCard extends Card {
        color: string
    }

    interface UserMessage {
        user: string
        message: string
    }
    
    interface Player {
        name: string
        avatar: string
        afk: boolean
        //cards: Card[]
    }
    
    interface unePlayer extends Player {
        pickingWildColor: boolean
        cards: UNOCard[]
    }

    interface LobbyUser {
      Name: string
      Avatar: string
    }
}