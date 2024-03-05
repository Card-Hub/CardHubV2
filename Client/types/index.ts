export { };

declare global {
    
    interface Card {
        id: number
        value: string
    }
    
    interface StandardCard {
        id: number
        value: string
        suit: string
    }
    
    interface UNOCard {
        id: number
        value: string
        color: string
    }

    interface UserMessage {
        user: string
        message: string
    }

}