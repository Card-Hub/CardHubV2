export { };

declare global {
    
    interface Card {
        id: number
        value: string
    }
    
    interface StandardCard extends Card {
        suit: string
    }
    
    interface UNOCard {
        id: number
        value: UnoValue
        color: UnoColor
    }

    interface UserMessage {
        user: string
        message: string
    }

}

export enum UnoColor {
    Black,
    Blue,
    Green,
    Red,
    Yellow
}

export enum UnoValue {
    Zero,
    One,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    DrawTwo,
    Reverse,
    Skip,
    SkipAll,
    Wild,
    WildDrawFour
}