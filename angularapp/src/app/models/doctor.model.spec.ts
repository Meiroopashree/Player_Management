import { Recipe } from './doctor.model';

describe('Recipe', () => {
  fit('Models_should_create_recipe_instance', () => {
    const recipe: Recipe = {
      recipeId: 1,
      name: 'Test Recipe',
      description: 'Test Description',
      ingredients: 'Test Ingredients',
      instructions: 'Test Instructions',
      author: 'Test Author'
    } as any;

    // Check if the recipe object exists
    expect(recipe).toBeTruthy();

    // Check individual properties of the recipe
    expect(recipe.recipeId).toBe(1);
    expect(recipe.name).toBeDefined();
    expect(recipe.description).toBe('Test Description');
    expect(recipe.ingredients).toBe('Test Ingredients');
    expect(recipe.instructions).toBe('Test Instructions');
    expect(recipe.author).toBe('Test Author');
});

});