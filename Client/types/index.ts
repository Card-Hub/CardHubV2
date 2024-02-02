export { };

declare global {
    interface Card {
        id: number
        value: string
        suit: string
    }

    interface UserMessage {
        user: string
        message: string
    }

    enum UserType {
        Gameboard,
        Player
    }

}