export { };

declare global {
    interface StandardCard {
        id: number
        value: string
        suit: string
    }

    interface UserMessage {
        user: string
        message: string
    }

}