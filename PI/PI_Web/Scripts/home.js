$(function(){
// left
	// 加载方案
		var MySolution=[
		  {
		    "Code": "string",
		    "Name": "string"
		  }
		];
		/*$.ajax({
		     url: "http://10.1.135.80:9999/Index/MySolution",
		     type: "GET",
		     data :MySolution,
		     dataType:'json',
		     beforeSend: function(XMLHttpRequest){
		     // console.log("beforeSend",XMLHttpRequest);
		     },
		     success: function(data){
		        for(var i=0,j=data.length;i<j;i++){
		        	$("#myOptions ul").append($('<li>'+data[i].Name+'</li>'));
		        };		        	    
		     },
		     complete: function(XMLHttpRequest){
		     // console.log("complete",XMLHttpRequest);
		     },
		     error: function(){
		     // console.log(XMLHttpRequest);
		     }
		 }); */
	// menu init	  			 
		$(".secondCon").hide(); 
		$(".thirdCon").hide(); 
		$(".secondCon").eq(0).show();
		$(".thirdCon").eq(0).show();
		$(".thirdCon").eq(4).show();
		$(".firstTit").addClass('iconHeadAct');	
		$(".secondTit").eq(0).addClass('iconAct');
		$(".thirdTit").eq(0).addClass('iconAct');	
		$(".thirdTit").eq(4).addClass('iconAct');	
	// menu change bind
		$("#leftBody").on("click",function(e){
			var target=e.target, 
				reg=new RegExp("Tit");			
			if (target.className) {
	 			if (reg.test(target.className)) {	 
		 			var titCls="."+target.className,
		 				conCls='.'+$(target).parent().find("div")[0].className,	
		 			    firstReg=new RegExp("first"),	
		 			    addCls="";
				 	if (firstReg.test(target.className)) {
				 		addCls="iconHeadAct";
				 		$(target).toggleClass(addCls);
				 		$(target).next().slideToggle();
				 	}else{
				 		addCls="iconAct";
				 		$(target).toggleClass(addCls);
				 		$(target).prevAll(titCls).removeClass(addCls);
				 		$(target).nextAll(titCls).removeClass(addCls);	 	
			 			$(target).next().slideToggle();
						$(target).prevAll(conCls).slideUp("slow");
					 	$(target).next().nextAll(conCls).slideUp("slow");
					};				 		
		 		};
	 		};	
		});
	// li mouseover
		var topLen=0;
		$(window).scroll(function (){
			topLen=$(this).scrollTop();		
		});
		// menu part
			$("#leftBody").on('mouseenter','li', function () {	
				if ($(this).parent().parent().parent()[0].tagName.toLowerCase()=="ol") {
					this.tip = this.title;
					$("body").append(
						'<div class="toolTipWrapper">'
							+'<p>'+$(this).attr("name")+'</p>'
							+'<p>'+this.tip+'</p>'
							+'<div></div>'
						+'</div>'
					);
					$('.toolTipWrapper').css({
						left:this.getBoundingClientRect().left,
						top:this.getBoundingClientRect().top+topLen-$('.toolTipWrapper').height()-10
					});
					this.title = "";	
					setTimeout(function(){
						$('.toolTipWrapper').fadeIn(10);
					},1000);	
					
				};	
			});
			$("#leftBody").on('mouseleave','li', function () {	
				if ($(this).parent().parent().parent()[0].tagName.toLowerCase()=="ol") {
					$('.toolTipWrapper').fadeOut(10);
					$('.toolTipWrapper').remove();
					this.title = this.tip;
				};	
			});
		// condition top part
			$("#checkedCon").on('mouseenter','a', function () {	
				if ($(this).parent().hasClass("oneFloor")) {
					var chineseRegex = /[\u4e00-\u9fa5]/g;
					var strName=$(this).attr("name");
					if (strName.replace(chineseRegex,"**").length>14) {
						$("body").append(
							'<div class="toolTipWrapper">'					
								+'<p>'+$(this).attr("name")+'</p>'
								+'<div></div>'
							+'</div>'
						);
						$('.toolTipWrapper').css({
							width:"150px",
							left:this.getBoundingClientRect().left,
							top:this.getBoundingClientRect().top+topLen-$('.toolTipWrapper').height()-10
						});	
						$('.toolTipWrapper div').css({
							"left":"69px"
						});	
						$('.toolTipWrapper').fadeIn(10);
					}; 
				};	
			});
			$("#checkedCon").on('mouseleave','a', function () {	
				if ($(this).parent().hasClass("oneFloor")) {
					$('.toolTipWrapper').fadeOut(10);
					$('.toolTipWrapper').remove();		
				};	
			});
		// custom top part
			$("#customWrap").on('mouseenter','a', function () {	
				if ($(this).parent().hasClass("oneFloor")) {
					var chineseRegex = /[\u4e00-\u9fa5]/g;
					var strName=$(this).attr("name");
					if (strName.replace(chineseRegex,"**").length>14) {
						$("body").append(
							'<div class="toolTipWrapper">'					
								+'<p>'+$(this).attr("name")+'</p>'
								+'<div></div>'
							+'</div>'
						);
						$('.toolTipWrapper').css({
							width:"150px",
							left:this.getBoundingClientRect().left,
							top:this.getBoundingClientRect().top+topLen-$('.toolTipWrapper').height()-10
						});	
						$('.toolTipWrapper div').css({
							"left":"69px"
						});	
						$('.toolTipWrapper').fadeIn(10);
					}; 
				};	
			});
			$("#customWrap").on('mouseleave','a', function () {	
				if ($(this).parent().hasClass("oneFloor")) {
					$('.toolTipWrapper').fadeOut(10);
					$('.toolTipWrapper').remove();		
				};	
			});		
// right
	// conditionRight
		// 指标类型  行高控制
			// $("body").on('mouseenter','.unitWrap', function () {
			// 	$(this).find("a").css({
			// 		"lineHeight":"18px",			
			// 	});
			// 	$(this).find("div").eq(0).css({
			// 		"display":"block",					
			// 	});
			// });
			// $("body").on('mouseleave','.unitWrap',function(){
			// 	$(this).find("a").css({
			// 		"lineHeight":"18px",
			// 	});
			// 	$(this).find("div").eq(0).css({
			// 		"display":"none"
			// 	});
			// });
		// 分布图
			var sliderFlag=false;
			$("#checkedWrap").on('mouseenter','.checkedConUnit', function () {			
				sliderFlag=true;
			});
			$("#checkedWrap").on('mouseleave','.checkedConUnit',function(){
                sliderFlag=false;
            });
		// 下拉列表
			// 鼠标事件
        	var classReg=new RegExp("divselect");
	        $(document).click(function(e){
	            if (e.target.parentNode) {	            
	            	if (e.target.parentNode.parentNode  && 
	            e.target.parentNode.parentNode.className &&
	            classReg.test(e.target.parentNode.parentNode.className.toLowerCase())) {
	            
	            	}else{
	            		$(".divselect ul").hide();
	            	}	            	            
	            }else{
	              $(".divselect ul").hide();
	            };	       
	            if (!sliderFlag) {
        	        $(".sliderWrap").css({
                        "display":"none"
                    });
	            };
	        }); 
         	// 按键事件
         	  var liNum=0;
	          $("#rightBody").on("mouseenter","li",function(){
	            liNum=$(this).index();
	            $(this).addClass("actLi");
	            var parent=$(this).parent().parent();
	            var len=parent.find("li").length;          
	            // 新增点击事件
	            	// $(this).click(function(){
	            	// 	if (liNum>1) {
	            	// 		sliderFlag=false;
	            	// 	};
	            	// });
	            // 上下左右键,分别是38,40,37,39;   13 enter 键   sliderFlag 控制组合插件显示、隐藏
	            $(document).keydown(function(event){ 
	              var e = event || window.event; 
	              var k = e.keyCode || e.which || e.charCode;               
	              if (k==38) {
	                if (liNum>0) {
	                  liNum--;
	                }else{
	                  liNum=0;
	                };
	                parent.find("li").removeClass("actLi");  
	                parent.find("li").eq(liNum).addClass("actLi");
	                return false;
	              }else if(k==40){
	                if (liNum<len-1) {
	                  liNum++;
	                }else{
	                  liNum=len-1;
	                };
	                parent.find("li").removeClass("actLi");  
	                parent.find("li").eq(liNum).addClass("actLi");
	                return false;
	              }else if(k==13){
	              	$(".divselect ul").hide();
	              	parent.find("span").eq(0).text(parent.find("li").eq(liNum).text());
	              };
	            });
	          });
	          $("#rightBody").on("mouseleave","li",function(){
	                $(this).removeClass("actLi");	                           
	                $(document).off("keydown");	             
	          });
// float
	// check back
		// init	
			$("#backCheckBody .checkCon").hide();			
			$("#backCheckBody .checkCon").eq(0).show();	
			$("#backCheckBody a").eq(0).addClass("checkAct");			
		 	$("#backCheckBody a").click(function(){
		 		$("#backCheckBody a").removeClass("checkAct");
		 		$(this).addClass("checkAct");
				$("#backCheckBody .checkCon").hide();
				$("#backCheckBody .checkCon").eq($(this).index()).show();
				return false;
			});	
			$(".checkTime span").eq(0).addClass("srcAct");	
			$(".checkTime span").click(function(){
				$(".checkTime span").removeClass("srcAct");
		 		$(this).addClass("srcAct");
				return false;
			});	
			$("#backCheck").css({
				"display":"block",
				"right":"-363px"
			});
		// change
			function closeBack(){
				$("#backCheck").animate({
						right:"-363px"
					},function(){
														
				});
				var backCheckObj=document.getElementById('backCheck');
				if (backCheckObj.getBoundingClientRect().left<$(window).width()) {
					hideMask();
				};
			}
			$("#checkSwt").click(function(){
				$("#backCheck").animate({					
						right:"0px"
					},function(){
						showMask();						
					});					
			});
			$("nav").click(function(){
				closeBack();				
			});
			$("#mask").click(function(){
				closeBack();				
			});
			$("#backCheckBtn").click(function(){			
				closeBack();					
			});
		// radio change
			$(".checkCon").on("click",".radioWrap",function(e){				
				var target=e.target;			
				if (target.nodeName.toLowerCase()=="span") {			
					$(target).parent().find("span").removeClass("selected");
					$(target).addClass("selected");
					if ($(target).index()==0) {
						$(".proCon").eq($(this).index()/2-1).css("display","block");
					}else{
						$(".proCon").eq($(this).index()/2-1).css("display","none");
					};
				};
			});
	// mask				
		function showMask(){     
	        $("#mask").css("height",$(document).height());     
	        $("#mask").css("width",$(document).width());     
	        $("#mask").show();     
	    };
    	//隐藏遮罩层  
	    function hideMask(){
	        $("#mask").hide();     
	    };
// oth	
	// input-ul#nameBody(搜索输入框)
		$('body').click(function(e){			
			if (e.target.nodeName.toLowerCase()=="input") {
				if (e.target.id.toLowerCase()=="indexname") {

				}else{
					$("#nameBody").hide();
				};
			}else if(e.target.nodeName.toLowerCase()=="li"){
				if (e.target.parentNode.parentNode.parentNode.id.toLowerCase()=="namebody") {
				
				}else{
					$("#nameBody").hide();
				};
			}else if(e.target.nodeName.toLowerCase()=="span"){
				if (e.target.parentNode.parentNode.parentNode.parentNode.id.toLowerCase()=="namebody") {

				}else{
					$("#nameBody").hide();
				}
			}else if(e.target.nodeName.toLowerCase()=="a"){
				if (e.target.className=="mCSB_buttonUp" || e.target.className=="mCSB_buttonDown") {

				}else{
					$("#nameBody").hide();
				}
			}else if(e.target.nodeName.toLowerCase()=="div"){
				if (e.target.className=="mCSB_dragger_bar" 
					|| e.target.className=="mCSB_draggerRail"
					|| e.target.className=="mCSB_draggerContainer") {
			
				}else{
					$("#nameBody").hide();
				}
			}else{
				$("#nameBody").hide();
			}
		}); 
		$("#indexName").keyup(function(){
			if ($(this).val()!="") {
				$("#clearBtn").css("display","block");
			}else{
				$("#clearBtn").css("display","none");
			}
		});

	// 自定义选股展开伸缩
		$(".customTit").click(function(){
	 		$(this).find("div").toggleClass("tabAct");
	 		$("#customWrap").toggle();	 		
			return false;
		});
	// 选股结果	
		$(".data-table-head").on("click","div",function(){		
			// $(this).toggleClass("customAct");
			// return false;
		});
});