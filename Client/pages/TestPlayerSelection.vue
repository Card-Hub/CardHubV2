<script setup lang="ts">
import { ref } from 'vue';

const isPlayer = ref<boolean>(true);

</script>

<template>
  <div>
    <div v-if="isPlayer" class="m-8">
      <h1>
        Une - New and Improved
      </h1>
      <p>
        Waiting for the host to start the game. Sit back and relax for now. While you're waiting, select an avatar.
      </p>

            <AvatarSelection />
    </div>
    <div v-else-if="!isPlayer" class="flex min-h-screen">

    </div>
    <div v-else>
      Error: No user type found
    </div>
  </div>
</template>

<style scoped>

.card {
  background: rgba( 255, 255, 255, 0.1 );
  box-shadow: 0 8px 32px 0 rgba( 74, 1, 29, 0.15);
  backdrop-filter: blur( 20px );
  -webkit-backdrop-filter: blur( 20px );
  border-radius: 28px;
  border: 1px solid rgba( 255, 255, 255, 0.10 );
}

.suit {
  opacity: 20%;
}
</style>

