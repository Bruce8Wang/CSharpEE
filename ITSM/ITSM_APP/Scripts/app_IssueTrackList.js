var filter, giftNo, giftName, beginQty, endQty, giftCategory;
var url = '/api/IssueTracks';
var userPermissionUrl = '/api/BD_UserPermission';

function ajaxHelper(uri, method, data) {
    return $.ajax({
        type: method,
        url: uri,
        async: false,
        dataType: 'json',
        contentType: 'application/json',
        //xhrFields: {
        //    withCredentials: true
        //},
        data: data ? JSON.stringify(data) : null
    }).fail(function (jqXHR, textStatus, errorThrown) {

    });
}


$(function () {
    getData('center');
    //var bCanExport = 0;
    ////获取用户权限
    //ajaxHelper(userPermissionUrl + "?$filter=FD_USER eq'" + localStorage.userId + "' and FD_OBJECTID eq 'rptGiftFetch'", "GET").done(function (data, status) {
    //    if (status == 'success') {
    //        if (data.length > 0) {
    //            for (var i = 0; i < data.length; i++) {
    //                if (data[i].FD_CANWRITE == 1) {
    //                    bCanExport = 1;
    //                    break;
    //                }
    //            }
    //        }

    //        if (bCanExport == 1) $('#btnExport').show();
    //    }
    //});
});

function getData(type) {
    var objList = '';
    switch (type) {
        case 'center':
            var data = [
            { "FD_ID": "1490d5fad0867db5c743703413790ce1", "FD_NAME": "公司领导" },
            { "FD_ID": "1490d5fad2875e959ab47bc4123ab0c3", "FD_NAME": "证券中心", },
            { "FD_ID": "1490d5fad66bbb1c3a932704ef6a64a4", "FD_NAME": "投融资中心" },
            { "FD_ID": "1490d5fad0836f9651e0ede46bb9fa9c", "FD_NAME": "设计院" },
            { "FD_ID": "1490d5fad08a1ac18afc64245ce80891", "FD_NAME": "总裁办公室" },
            { "FD_ID": "1490d5fad08c35cae32cc1d4006b3c8f", "FD_NAME": "总工办公室" },
            { "FD_ID": "14de57c007802c8be8d72aa452e81531", "FD_NAME": "品牌文化办公室" },
            { "FD_ID": "1490d5fad0811f1377d8c334458be5e8", "FD_NAME": "市场运营中心" },
            { "FD_ID": "1490d5fad188e335eb9ffec41bd90526", "FD_NAME": "供应链事业部" },
            { "FD_ID": "1490d5fad18dde6dff5cc4945eeb14be", "FD_NAME": "苗圃事业部" },
            { "FD_ID": "1490d5fad66fbd0ee59175340eb94ca7", "FD_NAME": "采购中心" },
            { "FD_ID": "1490d5fad28cae3fd3086a1466288e1c", "FD_NAME": "研发中心" },
            { "FD_ID": "1490d5fad3763d4a6868a1a49068c28a", "FD_NAME": "工程中心" },
            { "FD_ID": "1490d5fad76de546810130b47e28e5ee", "FD_NAME": "生态事业部" },
            { "FD_ID": "14d7592c161663e772d715444b4acf74", "FD_NAME": "生态旅游事业部" },
            { "FD_ID": "14e23ac8ffa4cd2c37b090745b8924ba", "FD_NAME": "生态金融发展中心" },
            { "FD_ID": "1490d5fad182733f4ef25524cec9848d", "FD_NAME": "预算管理中心" },
            { "FD_ID": "1490d5fad18dd04847327454a918c29b", "FD_NAME": "财务中心" },
            { "FD_ID": "1490d5fad37de0145520a1946bcafb6f", "FD_NAME": "审计监察中心" },
            { "FD_ID": "14eac9f5bf89a79c95a6d7c4984abfd0", "FD_NAME": "法务中心" },
            { "FD_ID": "14e4ac68697806795f57a084b9b88a52", "FD_NAME": "家庭园艺事业部" },
            { "FD_ID": "1490d5fad664690902fed844e7a9d600", "FD_NAME": "人力资源中心" },
            { "FD_ID": "1490d5fad08edcb4b2aff8c45d3b6afc", "FD_NAME": "流程与信息中心" },
            { "FD_ID": "1490d5fad37597ddcfd17134f61bde27", "FD_NAME": "北京区域中心" },
            { "FD_ID": "15090b523018dd7e50b728e484abc251", "FD_NAME": "海南区域中心" },
            { "FD_ID": "150af9b4b9ce4f171d8ad734551a805b", "FD_NAME": "珠海区域中心" },
            { "FD_ID": "1490d5fad183d285b165b4b402e804de", "FD_NAME": "行政中心" }
            ];
            objList = document.getElementById("selSubmitDept");
            objList.innerHTML = "";
            if (data.length > 0) {
                $("#selSubmitDept").append(generateHtmlOption(data, type));
            }
            break;

    }
    GenerateTable();
}


function query() {
    $('#lbNoData').hide();
    GenerateTable();
}

function exportExcel() {
    var objFilter = {
        GiftNo: $('#inputGiftNo').val(),
        GiftName: $('#inputGiftName').val(),
        BeginDate: $('#inputBeginDate').val(),
        EndDate: $('#inputEndDate').val(),
        GiftCategory: $('#gift-category').val(),
        ApplicantName: $('#inputApplicant').val(),
        ApplyDeptName: $('#inputApplyDept').val()
    };
    ajaxHelper('/api/ExportData', 'POST', objFilter).done(function (data) {
        if (data == null) $('#lbNoData').show();
        else window.open('../Export/' + data.FileName);
    });

}


//生成下拉框的HTML
function generateHtmlOption(dataArr, type) {
    var option = '<option value="' + "" + '">' + " " + '</option>';
    var arr = []; //定义一个数组
    var length = 0; //dataArr.length;
    switch (type) {
        case 'center':
            $.each(dataArr, function (key, val) {
                option = option + '<option value="' + val.FD_NAME + '">' + val.FD_NAME + '</option>';
            });
            break;
    }
    return option;
}

function GenerateTable() {
    module = $('#selModule').val();
    issueType = $('#selIssueType').val();
    beginDate = $('#inputBeginDate').val();
    endDate = $('#inputEndDate').val();
    submitter = $('#inputSumbitter').val();
    submitDept = $('#selSubmitDept').val();
    status = $('#selStatus').val();

    filter = "1 eq 1";

    if (submitter.length > 0) {
        filter = filter + ' and substringof(\'' + submitter + '\',Submitter)'
    }
    if (beginDate.length > 0) {
        //filter = filter + ' and FD_APPLYDATE ge \'' + beginDate + '\'';
        filter = filter + ' and FeedbackDate ge DateTime\'' + beginDate + 'T00:00:00\'';

    }
    if (endDate.length > 0) {
        //filter = filter + ' and FD_APPLYDATE le \'' + endDate + '\'';
        filter = filter + ' and FeedbackDate le DateTime\'' + endDate + 'T23:59:59\'';
    }
    if (issueType.length > 0) {
        filter = filter + ' and IssueType eq \'' + issueType + '\'';
    }
    if (submitDept.length > 0) {
        filter = filter + ' and SubmitDept eq \'' + submitDept + '\'';
    }

    if (status.length > 0) {
        filter = filter + ' and Status eq \'' + status + '\'';
    }

    var table = null;
    table = new TableView('table_div');
    table.header = {
        BillNo: '单据编号',
        Module: '模块',
        IssueType: '问题类型',
        Submitter: '业务提交人',
        SubmitDept: '业务提交部门',
        FeedbackDate: '反馈日期',
        HopeTime: '期望处理时间',
        Status: '处理状态'
    };

    table.dataKey = 'Id';
    table.pager.size = 15;
    ajaxHelper(url + '?$filter=' + filter, 'GET').done(function (data) {
        table.addRange(data);
    });
    table.display["count"] = true;
    table.display["filter"] = true;
    table.display["marker"] = true;
    table.display["pager"] = true;
    table.display["sort"] = true;
    table.display["multiple"] = true;
    table.render();
}

var TableView = function (id) {
    // TODO: 实现有序哈希类.
    /* 因为哈希表的实现可能是元素无序的, 所以使用数组代替. 为此, 定义了数据操作方法. */
    function array_index_of_key(arr, key, val) {
        for (var i in arr) {
            if (arr[i][key] == val) {
                return parseInt(i);
            }
        }
        return -1;
    }

    function array_index_of_item(arr, item) {
        for (var i in arr) {
            if (arr[i] == item) {
                return parseInt(i);
            }
        }
        return -1;
    }

    function array_get(arr, key, val) {
        var index = array_index_of_key(arr, key, val);
        if (index != -1) {
            return arr[index];
        }
        return false;
    }

    function array_del(arr, key, val) {
        var index = array_index_of_key(arr, key, val);
        if (index != -1) {
            var a1 = arr.slice(0, index);
            var a2 = arr.slice(index + 1);
            return a1.concat(a2);
        }
        return arr;
    }

    var self = this;
    this.id = id;
    this._rendered = false;
    this._filter_text = '';
    this.rows = [];
    this._display_rows = []; // 过滤后的数据集

    /**
	 * 当前控件所处的HTML节点引用.
	 * @type DOMElement
	 */
    this.container = null;

    /**
	 * 数据集的每一条记录的唯一标识字段名. 类似数据库表的主键字段名.
	 * @type String
	 */
    this.dataKey = '';
    /**
	 * 要显示的数据表格的标题.
	 * @type String
	 */
    this.title = '';
    /**
	 * 要显示的记录的字段, 以及所对应的字段名. 如 'id' : '编号'.
	 * @type Object
	 */
    this.header = {};
    /**
	 * 集成的分页控件, 可对表格中的数据集进行客户端分页.
	 * @type PagerView
	 */
    this.pager = {};
    /**
	 * 集成的排序控件, 用于显示分页按钮/链接.
	 * @type SortView
	 */
    this.sort = {};

    /**
	 * @class
	 * 用于确定要显示哪些内部控件, 控件对应的属性为Boolean类型, 取值为true时显示.
	 */
    function DisplayOptions() {
        /**
		 * 标题
		 * @type Boolean
		 */
        this.title = true;
        /**
		 * 计数
		 * @type Boolean
		 */
        this.count = true;
        /**
		 * 行选择框
		 * @type Boolean
		 */
        this.marker = true;
        /**
		 * 过滤器
		 * @type Boolean
		 */
        this.filter = false;
        /**
		 * 分页
		 * @type Boolean
		 */
        this.pager = false;
        /**
		 * 排序
		 * @type Boolean
		 */
        this.sort = false;
        /**
		 * 是否多选
		 * @type Boolean
		 */
        this.multiple = true;
        /**
		 * 调试
		 * @type Boolean
		 */
        this.debug = false;
    };

    /**
	 * 用于确定要显示哪些内部控件.
	 * @type TableView-DisplayOptions
	 */
    this.display = new DisplayOptions();

    /**
	 * 获取数据集指定id一条记录.
	 * @returns {Object} 数据集中的一条记录.
	 */
    this.get = function (id) {
        return array_get(this.rows, self.dataKey, id);
    };

    /**
	 * 添加一条记录, 如果控件已经被渲染, 会导致一次刷新.
	 * @param {Object} row: 记录对象.
	 */
    this.add = function (row) {
        var index = array_index_of_item(self.rows, row);
        if (index != -1) {
            return;
        }
        this.rows.push(row);
        this._display_rows.push(row);
        if (self._rendered) {
            self.render();
        }
    };


    /**
	 * 添加记录列表, 如果控件已经被渲染, 会导致一次刷新.
	 * 用本方法替代连续多次{@link TableView#add()}, 以提高性能.
	 * @param {Array[Object]} rows: 记录对象的数组.
	 */
    this.addRange = function (rows) {

        var index = {};
        for (var i in self.rows) {
            var rid = self.rows[i][self.dataKey];
            index[rid] = true;
        }

        for (var i in rows) {
            var row = rows[i];
            var rid = row[self.dataKey];

            if (!index[rid]) {
                this.rows.push(row);
                this._display_rows.push(row);
            }
        }
        if (self._rendered) {
            self.render();
        }
    };

    /**
	 * 删除一个记录对象, 如果控件已经被渲染, 会导致一次刷新.
	 * 可以在调用本方法前, 调用{@link TableView#get()}方法通过id获取要删除的记录对象.
	 * @param {Object} row: 记录对象.
	 */
    this.del = function (row) {
        var rid = row[self.dataKey];
        self.rows = array_del(self.rows, self.dataKey, rid);
        self._display_rows = array_del(self._display_rows, self.dataKey, rid);
        if (self._rendered) {
            self.render();
        }
    };

    /**
	 * 删除记录对象列表, 如果控件已经被渲染, 会导致一次刷新.
	 * 用本方法替代连续多次{@link TableView#del()}, 以提高性能.
	 * @param {Array[Object]} rows: 记录对象的数组.
	 */
    this.delRange = function (rows) {
        var index = {};
        for (var i in rows) {
            var rid = rows[i][self.dataKey];
            index[rid] = true;
        }

        var n_rows = [];
        for (var i in self.rows) {
            var row = self.rows[i];
            var rid = row[self.dataKey];
            if (!index[rid]) {
                n_rows.push(row);
            }
        }
        self.rows = n_rows;

        var n_rows = [];
        for (var i in self._display_rows) {
            var row = self._display_rows[i];
            var rid = row[self.dataKey];
            if (!index[rid]) {
                n_rows.push(row);
            }
        }
        self._display_rows = n_rows;


        if (self._rendered) {
            self.render();
        }
    };

    /**
	 * 内部方法. 用于全选或者取消全选行.
	 */
    //this._toggleSelect = function () {
    //    var c = $(self.container).find('th.marker input[type=checkbox]')[0];
    //    if (c.checked) {
    //        self.selectAll();
    //    } else {
    //        self.unselectAll();
    //    }
    //};

    /**
	 * 使用者重写本方法, 进行行双击回调.
	 * @param {int} id: 双击行的主键值.
	 * @event
	 */
    this.dblclick = function (id) {
        //alert(id);
        //storeHouse_edit(val.FD_ID, 550, '', '编辑', 'storeHouse-add.html?Id=' + val.FD_ID)
    };

    /**
	 * 内部方法, 行双击时调用.
	 */
    this._dblclick = function (id) {
        self.dblclick(id);
    };

    /**
	 * 获取当前可显示的数据数.
	 * @returns {int}
	 */
    this.rowCount = function () {
        var n = 0;
        for (var i in self._display_rows) {
            n++;
        }
        return n;
    };

    /**
	 * 内部方法, 渲染视图框架.
	 */
    this._render_framework = function () {
        var str = '';
        str += '<div class="TableView">\n';
        str += '<div class="datagrid_meta">\n';
        str += '<span class="title">' + this.title + '</span>';
        //str += '<span class="count">(<span class="marked_count">0</span>/<span class="row_count">0</span>)</span>';
        //str += '<span class="filter"><label>模糊过滤</label>';
        //str += '<input type="text" value="' + this._filter_text + '"'
        //    + ' onkeyup="document.getElementById(\'' + this.id + '\').view.filter(this.value)" />';
        //str += '</span>\n';
        str += '</div>\n';

        //var datagrid_div_id = self.id + '_datagrid_div__';
        //str += '<div class="datagrid_div" id="' + datagrid_div_id + '">\n';
        str += '<div class="datagrid_div">\n';
        str += '</div><!-- /.datagrid_div -->\n';

        var pager_id = self.id + '_pager__';
        str += '<div id="' + pager_id + '" class="pager"></div>\n';

        // debug
        var debug_div_id = self.id + '_debug';
        str += '<div id="' + debug_div_id + '"></div>\n';

        str += '</div><!-- /.TableView -->\n';

        var div = document.getElementById(self.id);
        div.view = self;
        self.container = div;
        self.container.innerHTML = str;

        // debug
        self._debug = $('#' + debug_div_id);

        // 捕获异常, 可以不需要PagerView工作
        try {
            self.pager = new PagerView(pager_id);
            self.pager.onclick = function (index) {
                self.render();
            };

            self.sort = new SortView();
            self.sort.onclick = function (field, order) {
                self.sort.sort(self._display_rows);
                self.render();
            };
        } catch (e) {
        }
    };

    self._render_framework();

    /**
	 * 更新统计数据.
	 */
    this._update_meta = function () {
        if (!self.display.count) {
            return;
        }
        var marked_count = 0;
        marked_count = $(self.container).find('table.datagrid td.marker input[value!=""]:checked').length;
        $(self.container).find('div.datagrid_meta span.marked_count').html(marked_count);
        $(self.container).find('div.datagrid_meta span.row_count').html(self.rowCount());
    }

    /**
	 * 内部方法. 绑定事件, 设置外观.
	 */
    this._after_render = function () {
        var trs = $(self.container).find('table.datagrid>tbody>tr.tv_row');
        trs.each(function (i, tr) {
            var cb = tr.getElementsByTagName('input')[0];

            var clz = i % 2 == 0 ? 'odd' : 'even';
            // 标记已选的行
            if (cb.checked) {
                clz += ' marked';
            }
            $(tr).removeClass('odd even marked');
            $(tr).addClass(clz);

            cb.onclick = function () {
                cb.checked = !cb.checked;
            };

            tr.onclick = function () {
                if (!cb.checked) {
                    if (!self.display.multiple) {
                        self.unselectAll();
                    }
                }
                cb.checked = !cb.checked;
                if (cb.checked) {
                    $(this).addClass('marked');
                } else {
                    $(this).removeClass('marked');
                }

                self._update_meta();
            };
            tr.onmouseover = function () {
                $(this).addClass('hover');
            };
            tr.onmouseout = function () {
                $(this).removeClass('hover');
            };
            tr.ondblclick = function () {
                self._dblclick(cb.value);
            };
        });

        self._update_meta();

        if (!self.display.title) {
            $(self.container).find('div.datagrid_meta span.title').hide();
        } else {
            $(self.container).find('div.datagrid_meta span.title').show();
        }
        if (!self.display.count) {
            $(self.container).find('div.datagrid_meta span.count').hide();
        } else {
            $(self.container).find('div.datagrid_meta span.count').show();
        }
        if (!self.display.filter) {
            $(self.container).find('div.datagrid_meta span.filter').hide();
        } else {
            $(self.container).find('div.datagrid_meta span.filter').show();
        }
        if (!self.display.pager) {
            $('#' + self.pager.id).hide();
        } else {
            $('#' + self.pager.id).show();
        }
        if (!self.display.marker) {
            $(self.container).find('td.marker, th.marker').hide();
        } else {
            $(self.container).find('td.marker, th.marker').show();
        }
    };

    // DEBUG
    function debug(str) {
        if (self.display.debug) {
            self._debug.css('border', '2px solid #f00');
            self._debug.append(str + '<br/>');
        }
    }

    /**
	 * 渲染控件.
	 */
    this.render = function (type) {
        var buf = [];
        buf.push('<table class="datagrid table-border table-bordered"><thead>\n');
        buf.push('<tr>\n');
        //buf.push('<th class="marker" width="5%" style="display:none" >');
        //if (self.display.multiple) {
        //    buf.push('<input type="checkbox" value="" onclick="document.getElementById(\'' + this.id + '\').view._toggleSelect()" />');
        //}
        //buf.push('</th>\n');

        var count = 0;
        for (var k in this.header) {
            count++;
        }
        var width = parseInt(100 / count);
        for (var k in this.header) {
            //buf.push('<th field="' + k + '" width="' + width + '%">' + self.header[k] + '</th>\n');
            //switch (self.header[k]) {
            //    case "FD_APPLYDATE":
            //        buf.push('<th field="' + k + '" width="' + 10 + '%">' + self.header[k] + '</th>\n');
            //        break;
            //    case "FD_GIFTNO":
            //    case "FD_UNITNAME":
            //    case "FD_PRICE":
            //    case "FD_QTY":
            //        buf.push('<th field="' + k + '" width="' + 5 + '%">' + self.header[k] + '</th>\n');
            //        break;
            //    default:
            //        buf.push('<th field="' + k + '" width="' + 8 + '%">' + self.header[k] + '</th>\n');
            //        break;
            //}

            buf.push('<th field="' + k + '" width="' + 8 + '%">' + self.header[k] + '</th>\n');
        }

        buf.push("</tr>\n</thead>\n");

        if (self.display.sort) {
            self.sort.sort(self._display_rows);
        }
        if (self.display.pager) {
            self.pager.itemCount = self._display_rows.length;
            var rows = self.pager.page(self._display_rows);
        } else {
            var rows = self._display_rows;
        }

        buf.push("<tbody>\n");

        for (var i in rows) {
            var row = rows[i];
            var rid = row[self.dataKey];
            buf.push('<tr class="tv_row">\n<td width="10" style="display:none" ><input type="checkbox" value="');
            buf.push(rid);
            buf.push('" /></td>\n');
            for (var k in self.header) {

                buf.push('<td><a href="IssueTrack.html?id=' + rid + '">');
                if (k == 'FeedbackDate' || k == 'HopeTime') { buf.push(row[k].toString().replace('T', ' ')); }
                else {
                    buf.push(row[k]);
                }
                buf.push('</a></td>\n');

                //buf.push('<td>');
                ////buf.push(row[k]);
                //if (k == 'FeedbackDate' || k == 'HopeTime') { buf.push(row[k].toString().replace('T', ' ')); }
                //else {
                //    buf.push(row[k]);
                //}
                //buf.push('</td>\n');
            }
            buf.push('</tr>\n');
        }


        buf.push("</tbody></table>\n");


        $(self.container).find('div.datagrid_meta span.title').html(this.title);

        // $.html() is really slow in IE
        //$(self.container).find('div.datagrid_div').html(buf.join(''));
        $(self.container).find('div.datagrid_div').each(function (i, e) {
            e.innerHTML = buf.join('');
        });

        self._after_render();

        if (self.display.pager) {
            self.pager.render();
        }

        if (self.display.sort) {
            var is_empty = true;
            if (self.sort.fields) {
                for (var i in self.sort.fields) {
                    is_empty = false;
                }
            }
            if (is_empty) {
                var fields = {};
                for (var k in self.header) {
                    fields[k] = [null, null];
                }
                self.sort.fields = fields;
            }
            var elements = {};
            $(self.container).find('table.datagrid th[field]').each(function (i, th) {
                var k = $(th).attr('field');
                if (k != undefined) {
                    elements[k] = th;
                }
            });
            self.sort.render(elements);
        }

        //$('.datagrid').fixedtableheader();

        self._rendered = true;
    };

    /**
	 * 设置所有行的选择标记. 如果设置了分页, 则只对当前页有效.
	 */
    this.selectAll = function () {
        $(self.container).find('th.marker input, td.marker input').each(function (i, e) {
            e.checked = true;
        });
        self._after_render();
    };

    /**
	 * 取消所有行的选择标记. 如果设置了分页, 则只对当前页有效.
	 */
    this.unselectAll = function () {
        $(self.container).find('th.marker input, td.marker input').each(function (i, e) {
            e.checked = false;
        });
        self._after_render();
    };

    /**
	 * 返回所有的记录的列表.
	 * @returns {Array[Object]}
	 */
    this.getDataSource = function () {
        return self.rows;
    }

    /**
	 * 获取所有标记为选择的行对应的记录的列表.
	 * @returns {Array[Object]}
	 */
    this.getSelected = function () {
        var items = [];
        $(self.container).find('.datagrid td.marker input[value!=""]:checked').each(function (i, cb) {
            if (cb.checked) {
                // 注意, 不要作为hash使用, 否则会导致使用者判断选中个数时错误.
                var row = array_get(self.rows, self.dataKey, cb.value);
                items.push(row);
            }
        });

        return items;
    };

    /**
	 * 获取所有已选择的数据对象键值的列表.
	 * @returns {Array[Key]}
	 */
    this.getSelectedKeys = function () {
        var keys = [];
        var rows = self.getSelected();
        for (var i in rows) {
            keys.push(rows[i][self.dataKey]);
        }
        return keys;
    };

    /**
	 * 进行模糊过滤.
	 * @param {String} text: Regex字符串.
	 */
    this.filter = function (text) {
        self._filter_text = text;
        self._display_rows = [];

        var regex = new RegExp(text, 'i');
        for (var key in self.rows) {
            var row = self.rows[key];
            if (text == '') {
                self._display_rows.push(row);
            } else {
                // 只对看到的进行过滤
                for (var k in self.header) {
                    var find = String(row[k]).replace(/<[^>]*>/g, '');
                    if (regex.test(find)) {
                        self._display_rows.push(row);
                        break;
                    }
                }
            }
        }

        if (self.display.pager) {
            self.pager.index = 1;
        }
        self.render('');
    };

    /**
	 * 清空所有行.
	 */
    this.clear = function () {
        self.rows = [];
        self._display_rows = [];

        if (self.display.pager) {
            self.pager.index = 1;
        }
        self.render('');
    };
}



