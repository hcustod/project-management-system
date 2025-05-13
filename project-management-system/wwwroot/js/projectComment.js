function loadComments(projectId) {
    $.ajax({
        url: `/ProjectManagement/ProjectComment/GetComments?projectId=${projectId}`,
        method: 'GET',
        success: function (data) {
            let commentsHtml = '';
            if (data.length > 0) {
                for (let i = 0; i < data.length; i++) {
                    commentsHtml += '<div class="comment border rounded p-2 mb-2">';
                    commentsHtml += `<p>${data[i].content}</p>`;
                    commentsHtml += `<small class="text-muted">Posted on: ${new Date(data[i].datePosted).toLocaleString()}</small>`;
                    commentsHtml += '</div>';
                }
            } else {
                commentsHtml = '<p>No comments yet.</p>';
            }
            $('#commentsList').html(commentsHtml);
        },
        error: function (xhr) {
            alert("Failed to load comments: " + xhr.statusText);
        }
    });
}

$(document).ready(function () {
    const projectId = $('#projectComments input[name="ProjectId"]').val();

    loadComments(projectId);

    $('#addCommentForm').submit(function (e) {
        e.preventDefault();

        const formData = {
            ProjectId: projectId,
            Content: $('#addCommentForm textarea[name="Content"]').val()
        };

        $.ajax({
            url: '/ProjectManagement/ProjectComment/AddComment',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(formData),
            success: function (res) {
                if (res.success) {
                    $('#addCommentForm textarea[name="Content"]').val('');
                    loadComments(projectId);
                } else {
                    alert(res.message || "Failed to post comment.");
                }
            },
            error: function (xhr) {
                alert("Error submitting comment: " + xhr.responseText);
            }
        });
    });
});
