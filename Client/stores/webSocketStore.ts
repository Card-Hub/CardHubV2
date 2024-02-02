import {defineStore} from 'pinia'
import {HttpTransportType, type HubConnection, HubConnectionBuilder, LogLevel} from '@microsoft/signalr'
import {ref} from 'vue'
import PlayingCard from "~/components/PlayingCard.vue";

export const useWebSocketStore = defineStore('webSocket', () => {

    const connection = ref<HubConnection | null>(null)
    const messages = ref<UserMessage[]>([])
    const cards = ref<Card[]>([]);
    const users = ref<string[]>([])
    const user = ref('')
    const room = ref('')

    // Allows Gameboard device to join Group
    const createRoom = async (): Promise<void> => {
        const { data } = await useApi<string>("game/createroom", { method: 'POST' });
        if (data.value) {
            room.value = data.value;
            console.log('Room created: ', room.value);
            await joinRoom(user.value, room.value, UserType.Gameboard);
        }
    }

    // Allows Player device to join Group
    const tryJoinRoom = async (): Promise<boolean> => {
        if (user.value && room.value) {
            const { data } = await useApi<boolean>(`game/verifycode/${ room.value }`, { method: 'GET' });
            if (data.value) {
                await joinRoom(user.value, room.value, UserType.Player);
                return true;
            }
            console.log('Invalid room code');
        }
        return false;
    }

    const joinRoom = async (user: string, room: string, userType: UserType): Promise<void> => {
        try {
            const runtimeConfig = useRuntimeConfig();
            const webSocketUrl = `${ runtimeConfig.public.baseURL }/gamehub`;
            const joinConnection = new HubConnectionBuilder()
                .withUrl(webSocketUrl, {
                    skipNegotiation: true,
                    transport: HttpTransportType.WebSockets
                })
                .configureLogging(LogLevel.Information)
                .build();

            joinConnection.on('ReceiveMessage', (userMessage: UserMessage) => {
                messages.value.push(userMessage);
            })

            joinConnection.on('ReceiveCard', (fromUser: string, card: Card) => {
                cards.value.push(card);
            })

            joinConnection.on('UsersInRoom', (users) => {
                users.value = users;
            })

            joinConnection.onclose(() => {
                connection.value = null;
                messages.value = [];
                users.value = [];
            })

            await joinConnection.start();
            await joinConnection.invoke('JoinRoom', { user, room, userType });
            connection.value = joinConnection;
        } catch (e) {
            console.log('HubConnection Error:', e);
        }
    }

    const sendCard = async (card: Card): Promise<void> => {
        try {
            if (connection.value !== null) { await connection.value.invoke('SendCard', card) }
        } catch (e) {
            console.log(e)
        }
    }

    const sendMessage = async (message: string): Promise<void> => {
        try {
            if (connection.value !== null) { await connection.value.invoke('SendMessage', message) }
        } catch (e) {
            console.log(e)
        }
    }

    const closeConnection = async (): Promise<void> => {
        try {
            if (connection.value !== null) { await connection.value.stop() }
        } catch (e) {
            console.log(e)
        }
    }

    return { connection, messages, cards, users, user, room,
        createRoom, tryJoinRoom, sendCard, sendMessage, closeConnection  }
})
