function changePatternSelect() {
	var patternForm = document.socPatternForm.socPropPattern;
	var selectPattern = patternForm.selectedIndex;
	switch (selectPattern){
		case 0: 
			$("#socGoalsForHome").addClass('graphActive');
			$("#socGoalsForAway").addClass('graphActive');
			
			$("#socGoalsAgainstHome").removeClass('graphActive');
			$("#socGoalsAgainstAway").removeClass('graphActive');
			
			$("#socTimeGoalsForHome").removeClass('graphActive');
			$("#socTimeGoalsForAway").removeClass('graphActive');
			
			$("#socTimeGoalsAgainstHome").removeClass('graphActive');
			$("#socTimeGoalsAgainstAway").removeClass('graphActive');
			
			$("#socPassSuccessHome").removeClass('graphActive');
			$("#socPassSuccessAway").removeClass('graphActive');
			
			$("#socPassDistanceHome").removeClass('graphActive');
			$("#socPassDistanceAway").removeClass('graphActive');
		
			break;
		case 1:
			$("#socGoalsForHome").removeClass('graphActive');
			$("#socGoalsForAway").removeClass('graphActive');
			
			$("#socGoalsAgainstHome").addClass('graphActive');
			$("#socGoalsAgainstAway").addClass('graphActive');
			
			$("#socTimeGoalsForHome").removeClass('graphActive');
			$("#socTimeGoalsForAway").removeClass('graphActive');
			
			$("#socTimeGoalsAgainstHome").removeClass('graphActive');
			$("#socTimeGoalsAgainstAway").removeClass('graphActive');
			
			$("#socPassSuccessHome").removeClass('graphActive');
			$("#socPassSuccessAway").removeClass('graphActive');
			
			$("#socPassDistanceHome").removeClass('graphActive');
			$("#socPassDistanceAway").removeClass('graphActive');
			
			break;
		case 2:
			$("#socGoalsForHome").removeClass('graphActive');
			$("#socGoalsForAway").removeClass('graphActive');
			
			$("#socGoalsAgainstHome").removeClass('graphActive');
			$("#socGoalsAgainstAway").removeClass('graphActive');
			
			$("#socTimeGoalsForHome").addClass('graphActive');
			$("#socTimeGoalsForAway").addClass('graphActive');
			
			$("#socTimeGoalsAgainstHome").removeClass('graphActive');
			$("#socTimeGoalsAgainstAway").removeClass('graphActive');
			
			$("#socPassSuccessHome").removeClass('graphActive');
			$("#socPassSuccessAway").removeClass('graphActive');
			
			$("#socPassDistanceHome").removeClass('graphActive');
			$("#socPassDistanceAway").removeClass('graphActive');
			
			break;
		case 3:
			$("#socGoalsForHome").removeClass('graphActive');
			$("#socGoalsForAway").removeClass('graphActive');
			
			$("#socGoalsAgainstHome").removeClass('graphActive');
			$("#socGoalsAgainstAway").removeClass('graphActive');
			
			$("#socTimeGoalsForHome").removeClass('graphActive');
			$("#socTimeGoalsForAway").removeClass('graphActive');
			
			$("#socTimeGoalsAgainstHome").addClass('graphActive');
			$("#socTimeGoalsAgainstAway").addClass('graphActive');
			
			$("#socPassSuccessHome").removeClass('graphActive');
			$("#socPassSuccessAway").removeClass('graphActive');
			
			$("#socPassDistanceHome").removeClass('graphActive');
			$("#socPassDistanceAway").removeClass('graphActive');
			
			break;
		case 4:
			$("#socGoalsForHome").removeClass('graphActive');
			$("#socGoalsForAway").removeClass('graphActive');
			
			$("#socGoalsAgainstHome").removeClass('graphActive');
			$("#socGoalsAgainstAway").removeClass('graphActive');
			
			$("#socTimeGoalsForHome").removeClass('graphActive');
			$("#socTimeGoalsForAway").removeClass('graphActive');
			
			$("#socTimeGoalsAgainstHome").removeClass('graphActive');
			$("#socTimeGoalsAgainstAway").removeClass('graphActive');
			
			$("#socPassSuccessHome").addClass('graphActive');
			$("#socPassSuccessAway").addClass('graphActive');
			
			$("#socPassDistanceHome").removeClass('graphActive');
			$("#socPassDistanceAway").removeClass('graphActive');
			
			break;
		case 5:
			$("#socGoalsForHome").removeClass('graphActive');
			$("#socGoalsForAway").removeClass('graphActive');
			
			$("#socGoalsAgainstHome").removeClass('graphActive');
			$("#socGoalsAgainstAway").removeClass('graphActive');
			
			$("#socTimeGoalsForHome").removeClass('graphActive');
			$("#socTimeGoalsForAway").removeClass('graphActive');
			
			$("#socTimeGoalsAgainstHome").removeClass('graphActive');
			$("#socTimeGoalsAgainstAway").removeClass('graphActive');
			
			$("#socPassSuccessHome").removeClass('graphActive');
			$("#socPassSuccessAway").removeClass('graphActive');
			
			$("#socPassDistanceHome").addClass('graphActive');
			$("#socPassDistanceAway").addClass('graphActive');
			
			break;
	}
};

	// soccor starters switch -----------
	$(function() {
		$("#socStSwitchHome").click(function() {
			$(this).addClass('active');
			$("#socStartersTabHome").addClass('active stTabActive');
			$("#socStSwitchAway").removeClass('active stTabActive');
			$("#socStartersTabAway").removeClass('active stTabActive');
			
			if($(".socStPowerSwitch-of").hasClass('active')){
				$(".socStPowerTab-of").addClass('active stTabActive');
				$(".socStPowerTab-df").removeClass('active stTabActive');
			}else{
				$(".socStPowerTab-df").addClass('active stTabActive');
				$(".socStPowerTab-of").removeClass('active stTabActive');
			}
		});
		$("#socStSwitchAway").click(function() {
			$(this).addClass('active');
			$("#socStartersTabAway").addClass('active stTabActive');
			$("#socStSwitchHome").removeClass('active stTabActive');
			$("#socStartersTabHome").removeClass('active stTabActive');
			
			if($(".socStPowerSwitch-of").hasClass('active')){
				$(".socStPowerTab-of").addClass('active stTabActive');
				$(".socStPowerTab-df").removeClass('active stTabActive');
			}else{
				$(".socStPowerTab-df").addClass('active stTabActive');
				$(".socStPowerTab-of").removeClass('active stTabActive');
			}
		});
	});
	
	// soccor starters power switch -----------
	$(function() {
		$(".socStPowerSwitch-of").click(function() {
			$(this).addClass('active');
			$(".socStPowerTab-of").addClass('active stTabActive');
			$(".socStPowerSwitch-df").removeClass('active');
			$(".socStPowerTab-df").removeClass('active stTabActive');
		});
		$(".socStPowerSwitch-df").click(function() {
			$(this).addClass('active');
			$(".socStPowerTab-df").addClass('active stTabActive');
			$(".socStPowerSwitch-of").removeClass('active');
			$(".socStPowerTab-of").removeClass('active stTabActive');
		});
	});

