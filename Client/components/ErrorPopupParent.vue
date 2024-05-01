<script setup lang="ts">
import { defineProps, defineEmits } from 'vue';

// https://vuejs.org/guide/typescript/composition-api.html
// this source apples to many of the components in this .vue file, playerhand.vue

//const props = defineProps<{
//  messages: {
//    type: string,
//    required: true
//    default: ["Can't fold right now!", "Not your turn!"]
//  };
//}>();

const messages = ref<string[]>(["Can't fold right now!", "Not your turn!"]);

const removeMessage = (message: string) => {
  let index = messages.value.indexOf(message);
  if (index != -1) { // message is in list
    messages.value = messages.value.slice(0,index).concat(messages.value.slice(index+1, messages.value.length));
  }
}

</script>

<template>
  <div class="parent">
    <div v-for="message in messages">
      <ErrorPopup :message="message" @remove="(m: string) => removeMessage(m)" />/>
    </div>
  </div>
</template>

<style scoped>
.parent {
  width: 20%;
  max-width: 200px;
  overflow: visible;
  background-color: white;
  height: 100vh;
}
</style>