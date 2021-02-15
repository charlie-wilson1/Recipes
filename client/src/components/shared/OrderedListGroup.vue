<template lang="html">
  <section class="indexed-list-group">
    <b-list-group>
      <b-list-group-item v-for="(value, index) in values" :key="index">
        <b-row>
          <b-col v-if="isIndexed" cols="1" class="list-group-index"
            >{{ ++index }}.</b-col
          >
          <b-col cols="10">{{ value }}</b-col>
        </b-row>
      </b-list-group-item>
    </b-list-group>
  </section>
</template>

<script lang="ts">
import { Component, Vue, Prop } from "vue-property-decorator";

@Component({})
export default class OrderedListGroup extends Vue {
  @Prop({ required: true })
  values!: string[];

  @Prop({
    required: true,
    default: false
  })
  isIndexed!: boolean;
}
</script>

<style scoped lang="scss">
.indexed-list-group {
  .list-group-item {
    border: none;
  }
}
</style>
