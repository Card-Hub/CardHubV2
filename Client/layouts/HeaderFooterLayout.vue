<!--JoinLayout.vue-->
<!-- copy-pastable usage: 
// note that #header and #footer template tags are optional
// and using them will overwrite default header and footer
// adding :headerDisabled=true or :footerDisabled=true to <NuxtLayout>
// will disable the header or footer, even if one is supplied as a template
// useful for disabling default header or footer

//pagename.vue

<template>
  <NuxtLayout name="header-footer-layout">
    <template #header></template>
    <template #content></template>
    <template #footer></template>
  </NuxtLayout>
</template
-->
<script setup lang="ts">
const props = defineProps(['headerDisabled', 'footerDisabled']);
</script>

<template>
  <div class="header" v-if="headerDisabled != true">
    <slot name="header">
    <!--By default, insert basic Header component-->
      <Header />
    </slot>
  </div>
  <div class="content">
    <slot name="content">Default #content content in HeaderFooterLayout</slot>
  </div>
  <div class="footer" v-if="footerDisabled != true">
    <!--By default, insert basic Footer component-->
    <Footer />
  </div>
</template>

<style scoped>
.header {
  display: block;
  width: 100%;
  height: 80px;
  margin:0;
}
.footer {
  display: block;
  width: 100%;
  height: 40px;
  position: absolute;
  bottom: 0;
  left: 0;
}
</style>
