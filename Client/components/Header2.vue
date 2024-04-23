<script setup lang="ts">
// https://primevue.org/menubar/
import Menubar from 'primevue/menubar';
import { ref } from 'vue';

const menuItems = ref([
  { label: 'Home', icon: 'pi pi-home', to: '/' },
  { label: 'Our Games', icon: 'pi pi-th-large', to: '/games' },
  { label: 'Join Game', icon: 'pi pi-users', to: '/join' },
  { label: 'Host Game', icon: 'pi pi-user-plus', to: '/games' }, // need to perhaps change this
  { label: 'About', icon: 'pi pi-info-circle', to: '/about' }
]);


const getLogo = () => {
  return new URL('../assets/icons/logos/combination.svg', import.meta.url);
};

const navigateToHome = async (): Promise<void> => {
  await navigateTo("/");
};
</script>

<template>
  <Menubar :model="menuItems">
<!--    to display the logo on the left side of the menubar-->
    <template #start>
      <img class="logo" :src="getLogo()" alt="Cardhub logo" @click="navigateToHome" />
    </template>
    
<!--    This is the actual menubar-->
    <template #item="{ item, props, root }">
      <NuxtLink :to="item.to" class="menuLink">
<!--        <span :class="item.icon"></span> HOPEFULLY WE CAN ADD ICONS HERE FOR MENU-->
        <span class="ml-2">{{ item.label }}</span>
        <Badge v-if="item.badge" :class="{ 'ml-auto': !root, 'ml-2': root }" :value="item.badge" />
        <span v-if="item.shortcut" class="ml-auto border-1 surface-border border-round surface-100 text-xs p-1">{{ item.shortcut }}</span>
      </NuxtLink>
    </template>
  </Menubar>
</template>

<style scoped>
.logo {
  width: 75px;
  margin: 0;
  padding-right: 10px;
  
}

.menuLink {
  display: flex;
  align-items: center;
  padding: 0.1rem 1rem;
  text-decoration: none;
  color: var(--text-color);
  font-size: 1.25rem;
  font-weight: 500;
  transition: background-color 0.3s;
}

</style>