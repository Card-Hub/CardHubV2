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

}