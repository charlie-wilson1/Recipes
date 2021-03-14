<template lang="html">
	<section class="create-list">
		<div class="text-right edit-button-container">
			<b-button
				variant="link"
				class="edit-button"
				@click="editingOrder = !editingOrder"
				>Edit Order
			</b-button>
		</div>
		<b-list-group>
			<b-list-group-item v-for="(value, index) in values" :key="index">
				<b-row>
					<b-col :md="editable ? '10' : '8'">
						{{ value.defaultValue }}
					</b-col>
					<b-col class="d-flex justify-content-between align-items-center">
						<span v-if="value.additionalValue" class="text-right mr-2">
							{{ value.additionalValue }}
						</span>
						<span v-if="!editingOrder" class="text-right">
							<b-icon
								icon="pencil"
								scale="1"
								v-if="editable"
								class="mr-2"
								@click="handleEdit(index)"
							></b-icon>
							<b-icon
								icon="trash"
								scale="1"
								v-if="deletable"
								@click="handleDelete(index)"
							></b-icon>
						</span>
						<span v-else class="text-right">
							<b-icon
								icon="arrow-up"
								scale="1"
								:disabled="index > 0"
								class="mr-2"
								@click="handleMove(index, index - 1)"
							></b-icon>
							<b-icon
								icon="arrow-down"
								scale="1"
								:disabled="index < values.length - 1"
								@click="handleMove(index, index + 1)"
							></b-icon>
						</span>
					</b-col>
				</b-row>
				<b-row v-if="value.notes" class="text-left">
					<b-col offset="1" cols="11" class="font-italic">
						{{ value.notes }}
					</b-col>
				</b-row>
			</b-list-group-item>
		</b-list-group>
	</section>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";

@Component({})
export default class CreateListItem extends Vue {
	@Prop({ required: false })
	values!: {
		defaultValue: string;
		additionalValue: string;
		notes: string;
	}[];

	@Prop({
		required: false,
	})
	editable!: boolean;

	@Prop({
		required: false,
	})
	deletable!: boolean;

	@Prop({ required: false })
	handleEdit!: Function;

	@Prop({ required: false })
	handleDelete!: Function;

	@Prop({ required: false })
	handleMove!: Function;

	public editingOrder = false;
}
</script>

<style scoped lang="scss">
.create-list {
	padding-top: 0px;

	.edit-button-container,
	.edit-button {
		margin-top: 0;
		padding-top: 0;
		padding-bottom: 0;
	}

	.list-group {
		margin-left: 35px;
		margin-top: 5px;
		width: 90%;
	}

	.list-group-item {
		border: none;
		padding-left: 0;
		padding-right: 0;
		background-color: transparent;
	}

	.text-right {
		margin-right: 0;
		margin-left: auto;
	}

	svg {
		padding-left: 0;
	}
}
</style>
