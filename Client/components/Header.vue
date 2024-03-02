<script setup lang="ts">

</script>

<template>
  <div class="header">
    <img src = "../assets/icons/logos/combination.svg" alt="Site logo" class="logo"/>
    <div class="buttons-outer">
      <div class="buttons">
        <NuxtLink href="/">
          <Button>Home</Button>
        </NuxtLink>
        <NuxtLink href="/">
          <Button>Our Games</Button>
        </NuxtLink>
        <NuxtLink href="/join">
          <Button>Join Game</Button>
        </NuxtLink>
        <NuxtLink href="/">
          <Button>Host Game</Button>
        </NuxtLink>
        <NuxtLink href="/">
          <Button>About</Button>
        </NuxtLink>
      </div>
    </div>
  </div>
</template>

<style scoped>
.header {
  padding: 10px;
  border-bottom: 2px solid black;
  height: 100%;
  background-color: var(--header-background-color);
  margin:0;
}
img {
  width:10%;
}
div.buttons-outer {
  width: 90%;
  height: 100%;
  /*background-color: yellow;*/
  display: inline-block;
}
div.buttons {
  display: flex;
  flex-direction: row;
  align-items:stretch;
  /*background-color: blue;*/
  justify-content: space-evenly;
}
Button {
  display: inline-block;
  height: 100%;
  width: 100%;
  
}
</style>