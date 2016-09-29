(function () {
    $(document).ready(function () {
        $('.cancel').on('click', function () {
            window.location.href = '/recipe/index';
        });

        $('form').on('submit', function (e) {
            e.preventDefault();

            var $form = $(this);

            if ($form.valid()) {
                var transmit = $form.find('input[type=submit]').data('transmit');

                if (transmit === 'create') {
                    saveRecipe();
                }
                else if (transmit === 'upgrade') {
                    var id = $form.find('input[name=id]').val();
                    updateRecipe(id);
                }
            }
        });

        $('.deleteBtn').on('click', function (e) {
            e.preventDefault();

            $('#deletePrompt').modal('show');
        });

        $('button[name=confirm').on('click', function (e) {
            e.preventDefault();

            var id = $('input[name=id]').val();
            deleteRecipe(id);
        })
    });

    function convertTextarea(text, field) {
        var newArray = [];
        var arrayText = text.split('\n').filter(function (e) { return e });

        for (var i = 0; i < arrayText.length; i++) {
            newArray.push({ [field]: arrayText[i] });
        }

        return newArray;
    }

    function saveRecipe() {
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

    function updateRecipe(id) {
        var name = $('#Name').val();
        var serves = $('#Serves').val();
        var ingredients = convertTextarea($('#Ingredients').val(), 'description');
        var method = convertTextarea($('#Method').val(), 'task');

        var request = {
            'id': id,
            'name': name,
            'serves': serves,
            'ingredients': ingredients,
            'method': method
        };

        console.debug(request);

        var recipe = $.ajax({
            contentType: "application/json; charset=utf-8",
            url: '/api/recipe/' + id,
            data: JSON.stringify(request),
            dataType: 'json',
            type: 'PUT',
        })
        .done(function () {
            window.location.href = '/recipe/detail/' + id;
        })
        .fail(function (jqXHR, error, errorThrown) {
            console.log(error);
        });
    }

    function deleteRecipe(id) {
        var url = '/api/recipe/' + id;

        var recipe = $.ajax({
            contentType: "application/json; charset=utf-8",
            url: url,
            type: 'DELETE',
        })
        .done(function (recipe) {
            console.debug(recipe);
            window.location.href = '/recipe/';
        })
        .fail(function (jqXHR, error, errorThrown) {
            console.log(error);
        });
    }
})();