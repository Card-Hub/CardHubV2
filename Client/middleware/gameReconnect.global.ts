import { storeToRefs } from "pinia";

// Check if the user is connected to the server and if not, try to reconnect
export default defineNuxtRouteMiddleware(async to => {
    if (!(to.path === "/join" || to.path.startsWith("/lobby") || to.path.startsWith("/gameboard") || to.path.startsWith("/player"))) {
        return;
    }

    console.log("Running middleware for", to.path);

    const store = useBaseStore();
    const { isBaseConnected} = storeToRefs(store);
    const { } = store;

    // if (!isConnected && cookieUser.value && cookieRoom.value) {
    //     console.log("Reconnecting in middleware");
    //     try {
    //         const joinedRoom = await tryJoinRoom();
    //         if (!joinedRoom) {
    //             navigateTo("/join");
    //         }
    //     } catch (error) {
    //         console.log("Unable to connect in middleware:", error);
    //     }
    // }

    if (!isBaseConnected) {
        navigateTo("/join");
    }

});