<script setup lang="ts">

</script>

<template>
  <div class="une">
    <NuxtLink href="/games" class="go-back-btn">
      <Button>Go Back</Button>
    </NuxtLink>

    <div class="column">
      <h1>UNO</h1>
      
    </div>
    
    <div class="column">
      <h1>UNO</h1>
    </div>
    
    <div class="column">
      <h1>UNO</h1>
    </div>
    
  </div> 
</template>

<style scoped>
.une {
  display: flex;
  flex-direction: column;
  justify-content: center;
  height: 100%;
  width: 100%;

}

.column {
  flex: 1;
  margin: 0 10px;
}

.go-back-btn {
  padding-left: 20px;
  padding-top: 20px;
}
</style>