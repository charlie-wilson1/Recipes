<template lang="html">
	<section class="indexed-list-group">
		<b-list-group>
			<b-list-group-item v-for="(value, index) in values" :key="index">
				<b-row>
					<span v-if="isIndexed" class="list-group-index">{{ ++index }}.</span>
					<span>{{ value }}</span>
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
		default: false,
	})
	isIndexed!: boolean;
}
</script>

<style scoped lang="scss">
.indexed-list-group {
	.list-group-item {
		border: none;
		font-size: 1.2em;

		.list-group-index {
			width: 1.5em;
			margin-right: 1.25%;
		}
	}
}
</style>
