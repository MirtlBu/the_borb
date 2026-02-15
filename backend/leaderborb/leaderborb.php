<?php
/**
 * Plugin Name: Leaderborb
 * 
 * Put into /home/jutskatf/public_html/wp-content/plugins/leaderborb/
 */

add_action('rest_api_init', function () {
    register_rest_route('borb', '/leaderboard', array(
        'methods'  => 'GET',
        'callback' => 'get_leaderboard',
        'permission_callback' => '__return_true'
    ));
    register_rest_route('borb', '/leaderboard', array(
        'methods'  => 'POST',
        'callback' => 'add_to_leaderboard',
        'permission_callback' => '__return_true',
    ));
});

global $leaderborb_table_name;
global $wpdb;
$leaderborb_table_name = $wpdb->prefix . 'leaderborb';

function get_leaderboard() {
    global $wpdb, $leaderborb_table_name;

    $results = $wpdb->get_results(
        "SELECT player_name, score 
         FROM $leaderborb_table_name 
         ORDER BY score DESC 
         LIMIT 10",
        ARRAY_A
    );
    
    $leaderboard = array_map(function($row) {
        return array(
            'playerName' => $row['player_name'],
            'score'      => $row['score']
        );
    }, $results);

    return array(
        'leaderboard' => $leaderboard
    );
}

function add_to_leaderboard($request) {
    global $wpdb, $leaderborb_table_name;

    // Get the JSON body
    $data = $request->get_json_params();

    if (!isset($data['playerName']) || !isset($data['score'])) {
        return new WP_Error('missing_data', 'playerName and score are required', array('status' => 400));
    }

    $player_name = sanitize_text_field($data['playerName']);
    $score = intval($data['score']);
    
    $query = $wpdb->prepare(
        "INSERT INTO $leaderborb_table_name (player_name, score)
         VALUES (%s, %d)
         ON DUPLICATE KEY UPDATE score = GREATEST(score, VALUES(score))",
        $player_name,
        $score
    );

    $result = $wpdb->query($query);

    if ($result === false) {
        return new WP_Error('db_error', 'Failed to update leaderborb', array('status' => 500));
    }
    
    $result = $wpdb->query(
        "DELETE FROM $leaderborb_table_name
         WHERE player_name NOT IN (
            SELECT player_name FROM (
                SELECT player_name
                FROM $leaderborb_table_name
                ORDER BY score DESC
                LIMIT 10
            ) AS top_players
         )"
    );
    
    if ($result === false) {
        return new WP_Error('db_error', 'Failed to cleanup leaderborb', array('status' => 500));
    }

    return array(
        'success' => true,
        'playerName' => $player_name,
        'score' => $score
    );
}

register_activation_hook(__FILE__, 'leaderborb_create_table');

function leaderborb_create_table() {
    global $wpdb, $leaderborb_table_name;

    $charset_collate = $wpdb->get_charset_collate();

    $sql = "CREATE TABLE $leaderborb_table_name (
        player_name VARCHAR(100) NOT NULL,
        score INT NOT NULL,
        PRIMARY KEY  (player_name)
    ) $charset_collate;";

    require_once(ABSPATH . 'wp-admin/includes/upgrade.php');
    dbDelta($sql);
}