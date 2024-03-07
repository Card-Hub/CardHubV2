import { onBeforeMount, onBeforeUnmount } from 'vue';

export const useGradientBackground = function () {
    let originalStyle = "";
    const applyGradient = function () {
        document.body.style.background = "linear-gradient(20deg, #000000 0%, #313134 100%)";
    };

    onBeforeMount(() => {
        originalStyle = document.body.style.background;
        console.log("styleeeeeee", originalStyle)
        applyGradient();
    })
    onBeforeUnmount(() => {
        document.body.style.background = "";
    })
};