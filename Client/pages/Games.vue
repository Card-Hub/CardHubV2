<script setup lang="ts">
import {ref} from 'vue';
import Header2 from "../components/Header2.vue";

// https://primevue.org/iconfield/
import IconField from 'primevue/iconfield';
import InputIcon from 'primevue/inputicon';


const getLogo = () => {
  return new URL('../assets/icons/logos/combination.svg', import.meta.url);
};

interface Game {
  title: string;
  image: URL;
  deckColor: string;
  link: string;
}

const popularGames = [
  {
    title: 'UNE', // Uné
    image: new URL('../assets/icons/unoDeck/UNE.svg', import.meta.url),
    deckColor: '#151515',
    link: '/games/une'
    },
    {
      title: 'Blackjack',
      image: new URL('../assets/icons/standardDeck/spades.svg', import.meta.url),
      deckColor: 'white',
      link: '/games/blackjack'
    },
    {
      title: 'Texas Hold Em',
      image: new URL('../assets/icons/standardDeck/hearts.svg', import.meta.url),
      deckColor: 'white',
      link: '/games/texasholdem'
  }]; // add more as necessary

const searchQuery = ref('');
const searchResult = ref(null);

const searchGame = () => {
  if (searchQuery.value) {
    searchResult.value = popularGames.find(game => game.title.toLowerCase() === searchQuery.value.toLowerCase());
  } else {
    searchResult.value = null;
  }
};

</script>

<template>
  <Header2/>
  <div id="decklibrary-page">
    <img class="cd-logo" alt="Cardhub logo"
         :src='getLogo()'/>

    <IconField iconPosition="left" class="bottom-5">
      <InputIcon class="pi pi-search"></InputIcon>
      <InputText v-model="searchQuery" @input="searchGame" placeholder="Search for games"/>
    </IconField>

    <div v-if="searchResult">
      <div class="flex flex-wrap justify-start items-start">
        <div
             :key="searchResult.title" class="m-2">
          <NuxtLink :to="searchResult.link">
            <div class="flex justify-center items-center w-28 h-44 rounded-md shadow-md mb-2" :style="{ backgroundColor: searchResult.deckColor }">
              <img :src="searchResult.image"
                   alt="game icon"
                   class="deck-view"/>
            </div>
          </NuxtLink>
          <div class="text-1xl"> {{ searchResult.title }} </div>
        </div>
      </div>
      
    </div>
    <div v-else>
      <p v-if="searchQuery" class="text-red-600">Game Not Found</p>
      <div v-else>
        <h1 class="text-3xl mt-8 mb-4 text-white">Made by CardHub</h1>

        <div class="flex flex-wrap justify-start items-start">
          <div v-for="game in popularGames"
               :key="game.title" class="m-2">
            <NuxtLink :to="game.link">
              <div class="flex justify-center items-center w-28 h-44 rounded-md shadow-md mb-2" :style="{ backgroundColor: game.deckColor }">
                <img :src="game.image"
                     alt="game icon"
                     class="deck-view"/>
              </div>
            </NuxtLink>
            <div class="text-1xl"> {{ game.title }} </div>
          </div>
        </div>
      </div>
      
        
    </div>
    
    

  </div>

</template>

<style scoped>
#decklibrary-page {
  background: linear-gradient(20deg, #000000 0%, #313134 100%);
  width: 100%;
  height: 100vh;
  display: flex;
  flex-direction: column;
  align-items: center;
}

.cd-logo {
  padding-top: 50px;
  padding-bottom: 50px;
  width: 30%;
}

.deck-view {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  height: 50%;
}

</style>