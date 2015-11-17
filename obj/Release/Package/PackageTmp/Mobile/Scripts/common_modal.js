$(function() {
	//Modal common header
	$("#modalBtn-open").click(
	 function(){
	  $("#modalOverlay").fadeIn("fast");
	  $("#modalContents").fadeIn("fast");
	 }
	);
	$("#modalBtnPreLogin").click(
	 function(){
	  $("#modalContents").fadeOut("fast");
	  $("#modalContents02").fadeIn("fast");
	 }
	);

	$(".modalBtn-close").click(
	 function(){
	  $("#modalOverlay").fadeOut("fast");
	  $("#modalContents").fadeOut("fast");
	  $("#modalContents02").fadeOut("fast");
	  $("#modalSocExp").fadeOut("fast");
	 }
	);
	$("#modalBtnLogin").click(
	 function(){
	  $("#modalOverlay").fadeOut("fast");
	  $("#modalContents").fadeOut("fast");
	  $("#modalContents02").fadeOut("fast");
	 }
	);
	$("#modalOverlay").click(
	 function(){
	  $("#modalOverlay").fadeOut("fast");
	  $("#modalContents").fadeOut("fast");
	  $("#modalContents02").fadeOut("fast");
	  $("#modalSocExp").fadeOut("fast");
	  $("#modalSocExpDelete").fadeOut("fast");
	 }
	);

	// Modal expected
	$(document).ready(function() {
		$(".modalSocExpBtn-open").click(
			function(){
				$("#modalOverlay").fadeIn("fast");
				$("#modalSocExp").fadeIn("fast");
			}
		);
		$(".modalSocExpBtn-close").click(
			function() {
				$("#modalOverlay").fadeOut("fast");
				$(".modalContents").fadeOut("fast");
				$("#modalBtn-exp").fadeOut("fast");
			}
		);

	// Modal expected -> delete
	$("#socPanelEditExp").click(
		function() {
			$("#modalSocExp").fadeOut("fast");
			$("#modalSocExpDelete").fadeIn("slow");
			}
		);

	// expected delete toast
	$("#expDelete").click(
		function() {
				$("#modalOverlay").fadeOut("fast");
				$("#modalSocExpDelete").fadeOut("fast");
				$("#toastSocExp").fadeIn("slow");
			setTimeout(function() {
			$("#toastSocExp").fadeOut("slow");
		}, 2000);
		}
	);
});
});