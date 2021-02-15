import Vue from "vue";
import Vuelidate from "vuelidate";
import { BootstrapVue, IconsPlugin } from "bootstrap-vue";
import App from "./App.vue";
import router from "./router";
import store from "./store/store";
import vSelect from "vue-select";
import VueToast from "vue-toast-notification";
import "vue-toast-notification/dist/theme-sugar.css";
import axios from "axios";

Vue.use(Vuelidate);
Vue.use(BootstrapVue);
Vue.use(IconsPlugin);
Vue.use(VueToast);
Vue.component("vue-select", vSelect);

new Vue({
  router,
  store,
  render: h => h(App)
}).$mount("#app");
