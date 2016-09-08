(function () {
    function convertTextarea(text, field) {
        var newArray = [];
        var arrayText = text.split('\n').filter(function (e) { return e });

        for (var i = 0; i < arrayText.length; i++) {
            newArray.push({ [field] : arrayText[i] });
        }

        return newArray;
    }

    $(document).ready(function () {
        $('form').on('submit', function (e) {
            e.preventDefault();

            var $form = $(this);

            if ($form.valid()) {
                // TODO Ajax call

                var name = $('#Name').val();
                var serves = $('#Serves').val();
                var ingredients = convertTextarea($('#Ingredients').val(), 'description');
                var method = convertTextarea($('#Method').val(), 'task');

                var request = {
                    'name': name,
                    'serves': serves,
                    'ingredients': ingredients,
                    'method': method
                };
                
                console.debug(request);

                var recipe = $.ajax({
                    contentType: "application/json; charset=utf-8",
                    url: '/api/recipe',
                    data: JSON.stringify(request),
                    dataType: 'json',
                    type: 'POST',
                })
	            .done(function (recipe) {
	                console.debug(recipe);
	                window.location.href = '/recipe/detail/' + recipe.id;
	            })
	            .fail(function (jqXHR, error, errorThrown) {
	                console.log(error);
	            });
            }
        });
    });
})();