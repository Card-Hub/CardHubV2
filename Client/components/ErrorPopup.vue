<script setup lang="ts">
import { defineProps, defineEmits } from 'vue';

// https://vuejs.org/guide/typescript/composition-api.html
// this source apples to many of the components in this .vue file, playerhand.vue

const props = defineProps<{
  message: {
    type: string,
    required: true
  };
}>();

const emit = defineEmits(['remove']);
const remove = () => {
  emit("remove", props.message);
  clearTimeout(timeoutId);
}

let timeoutId = setTimeout(remove, 3000);

</script>

<template>
  <div class="popup">
    <Button @click="remove"> <span class="pi pi-times"></span></Button>
    <p> {{ message }}</p>
  </div>
</template>

<style>
.popup {
  background-color: var(--cardhub-red);
  width: 100%;
  padding: 2em;
  color: white;
}
</style>