module.exports = {
	devServer: {
		overlay: {
			warnings: false,
			errors: true,
		},
	},
	lintOnSave: process.env.NODE_ENV !== "production",
};
