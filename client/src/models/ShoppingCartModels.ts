export interface ShoppingCartItem {
	name?: string;
	quantity?: number;
	unit?: string;
}

export interface ShoppingCart {
	id?: string;
	owner: string;
	items: ShoppingCartItem[];
}

export interface UpdateShoppingCartItem {
	name?: string;
	quantity?: number;
}
