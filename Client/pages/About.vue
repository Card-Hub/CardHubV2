<script setup lang="ts">

  import Header from "~/components/Header.vue";

  const getLogo = () => {
    return new URL('../assets/icons/logos/combination.svg', import.meta.url);
  };

  const getPearl = () => {
    return new URL('../assets/icons/logos/pearl.svg', import.meta.url);
  };

</script>

<template>
<!--  <div class="flex flex-wrap justify-center items-center">-->
  <Header2 />
  <div class="all-info">
    <br> <br>
    
    <div class="starter">
      <img class="cardhub-logo" alt="Cardhub logo" 
           :src= 'getLogo()' />
      <h3 class="basic-info"> CS 426 Senior Projects | Spring 2024
        <br> University of Nevada, Reno | CSE Department
        <!--      <img alt="Pearl logo" src="../assets/icons/logos/pearl.svg" style="width: 50px; height: 50px;"/>-->
        <br> - Team 6: Pearl -
        <br> Alex Mozqueda | Rubi Dionisio | Liam Prior | Lyssie Brown
        <br> - Instructors -
        <br> Dave Feil-Seifer, Devrin Lee, Sara Davis
        <br> - External Advisor -
        <br> Erin Keith, CSE Department
      </h3>
    </div>
    
    <div class="about-cd">
      <h2> <br> <br> About Cardhub </h2>

      <p class="project-description">
        CardHub is an online multiplayer card game, offering a diverse array of genres playable anywhere with an internet connection. Inspired by Kahoot, Jackbox, and Tabletop Simulator, our game allows players to create private rooms from a central device, functioning as a gameboard akin to a table with crucial game information. Player devices, such as phones or tablets, act as interactive hands, enabling actions like drawing, placing, and reordering cards. This project aims to revolutionize the gaming experience, combining inspiration from popular multiplayer games.
        <br><br> CardHub’s main goal is to create a game platform that allows the playing of card games without need for physical cards. There has yet to be an application that unifies different types of games. A product like this would be nice to have as the go-to app for party games that allow for its users to truly make it their own. While we recognize that it is out of the scope of a 426 project, our end goal of CardHub is to allow users to create their own games, with their own custom cards and functionality.
        <br><br> Cardhub is intended for anyone who likes playing card games and has some device to host a game and/or another device to play the games. The audience also includes those who cannot afford buying cards as well as those who don’t like carrying multiple decks all the time. We expect our audience to gather around a central location, like a kitchen table, so we will be emphasizing mobile devices in our design.

      </p>
    </div>
    
    <br> <br>
    
    <div class="team">
      <img alt="Team Pearl" 
           :src="getPearl()" style="width: 300px; height: 90px;"/>
      <h2> Meet the Team! </h2>
      <p> Pictures to be uploaded later </p>
    </div>
    
    <br> <br>
    
    <div class="sources">
      <h2> Project Related Resources </h2>
      <p class="resources"> 
        Licensing information:
        <br> <a href="https://github.com/vuejs/vue/blob/main/LICENSE"> Vue’s MIT license </a>
        <br> <a href="https://github.com/nuxt/nuxt/blob/main/LICENSE"> Nuxt’s MIT license </a>
        <br> <a href="https://github.com/primefaces/primevue/blob/master/LICENSE.md"> PrimeVue MIT license </a>
        <br> <a href="https://medium.com/made-with-creative-commons/card-against-humanity-8c0cc2c6c299"> Cards Against Humanity licensing information and context </a>  
        <br> <a href="https://creativecommons.org/licenses/by-nc-sa/4.0/"> Creative Commons Attribution-NonCommercial-ShareAlike 4.0 license </a>
        <br> <a href="https://learn.microsoft.com/en-us/dotnet/"> .net C# documentation </a> 
        <br> <a href="https://www.w3.org/WAI/WCAG22/quickref/"> WCAG Guidelines </a> </p>
    </div>
  </div>
  <Footer />
</template>

<style scoped>
  .about-cd{
    text-align: left;
    line-height: 30px;
    margin-left: 70px;
    margin-right: 70px;
    color: white;
  }

  .sources{
    text-align: left;
    line-height: 30px;
    margin-left: 70px;
    margin-right: 70px;
  }

  .resources{
    text-align: left;
    line-height: 30px;
    margin-left: 40px;
    color:white;
  }
  
  .all-info{
    text-align: center;
    line-height: 30px;
  }

  .basic-info{
    text-align: center;
    line-height: 30px;
  }
  
  .project-description{
    text-align: left;
    line-height: 30px;
    color: white;
  }
  
  .cardhub-logo{
    width: 400px;
    height: 200px;
  }

</style>