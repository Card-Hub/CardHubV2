import { useNuxtApp } from "nuxt/app";

const nuxtApp = useNuxtApp();
const getToast = () => nuxtApp.vueApp.config.globalProperties.$toast
const toast = {
    add(msg: any) {
        getToast().add(msg)
    }
}
export default toast